using System.Collections;
using System.Text;
using System;

using UnityEngine.Networking;
using UnityEngine;

using DialogFlow.Utilities;

namespace DialogFlow
{

    public static class DialogFlowAuth
    {
        private static DateTime _expires;
        private static string _token;

        public static IEnumerator GetToken(Action<string> callback)
        {
            _expires = DateTime.MinValue;
            
            if (_expires > DateTime.Now)
            {
                callback(_token);
                yield break;
            }

            string jwt = Jwt.GetJwt(Constants.Email, Constants.Keyfile, Constants.Scope);

            WWWForm form = new WWWForm();
            form.AddField("grant_type", Constants.Grant);
            form.AddField("assertion", jwt);
            
            UnityWebRequest request = UnityWebRequest.Post(Constants.Url, form);

            yield return request.SendWebRequest();
            
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                callback(null);
                yield break;
            }

            byte[] rawResponse = request.downloadHandler.data;
            string jsonResponse = Encoding.UTF8.GetString(rawResponse);
            AuthResponseBody response = Json.FromJson<AuthResponseBody>(jsonResponse);

            _expires = DateTime.Now.AddSeconds(response.expiresIn);
            _token = response.accessToken;

            callback(response.accessToken);
        }
    }
}
