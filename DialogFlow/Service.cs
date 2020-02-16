using System.Collections;
using System.Text;
using System;

using UnityEngine.Networking;
using UnityEngine;

using DialogFlow.Utilities;
using Avatars.Enum;

namespace DialogFlow
{
    public class DialogFlowService
    {
        private readonly string _projectId;
        private readonly string _sessionId;

        public DialogFlowService()
        {
            // Set Project Id from DialogFlow
            _projectId = "";
            _sessionId = Guid.NewGuid().ToString();
        }

        public IEnumerator DetectIntent(AudioClip audio, Gender gender, Action<AudioClip> callback)
        {
            string url = $"https://dialogflow.googleapis.com/v2/projects/{_projectId}/agent/sessions/{_sessionId}:detectIntent";
            string inputAudio = Convert.ToBase64String(Audio.FromAudioClip(audio));
            
            if (inputAudio == "")
            {
                callback(null);
                yield break;
            }
            
            RequestBody requestBody = GetRequestBody(inputAudio, GetSSMLGender(gender));
            string jsonBody = Json.ToJson(requestBody);
            byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

            UnityWebRequest request = new UnityWebRequest(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(rawBody),
                downloadHandler = new DownloadHandlerBuffer(),
            };

            string accessToken = "";
            yield return DialogFlowAuth.GetToken((string token) => accessToken = token);

            request.SetRequestHeader("Authorization", "Bearer " + accessToken);

            yield return request.SendWebRequest();
            
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                callback(null);
                yield break;
            }

            byte[] rawResponse = request.downloadHandler.data;
            ResponseBody response = GetResponseBody(rawResponse);
            
            AudioClip outputAudio = response.outputAudio != null ? 
                Audio.ToAudioClip(Convert.FromBase64String(response.outputAudio)) :
                Audio.EmptyClip();

            callback(outputAudio);
        }

        private string GetSSMLGender(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return SSMLGender.Male;
                case Gender.Female:
                    return SSMLGender.Female;
                case Gender.Neutral:
                    return SSMLGender.Neutral;
                default:
                    return SSMLGender.Unspecified;
            }
        }

        private RequestBody GetRequestBody(string inputAudio, string gender)
        {
            InputAudioConfig inputAudioConfig = new InputAudioConfig 
            {
                audioEncoding = InputAudioEncoding.Linear16,
                sampleRateHertz = 16000,
                languageCode = "nl-NL",
            };

            VoiceConfig voiceConfig = new VoiceConfig
            {
                ssmlGender = gender,
            };

            SynthesizeSpeechConfig synthesizeSpeechConfig = new SynthesizeSpeechConfig
            {
                voice = voiceConfig,
            };

            OutputAudioConfig outputAudioConfig = new OutputAudioConfig
            {
                audioEncoding = OutputAudioEncoding.Linear16,
                synthesizeSpeechConfig = synthesizeSpeechConfig,
            };

            QueryInput queryInput = new QueryInput
            {
                audioConfig = inputAudioConfig,
            };

            RequestBody requestBody = new RequestBody
            {
                outputAudioConfig = outputAudioConfig,
                queryInput = queryInput,
                inputAudio = inputAudio,
            };

            return requestBody;
        }

        private ResponseBody GetResponseBody(byte[] response)
        {
            string json = Encoding.UTF8.GetString(response);
            
            return Json.FromJson<ResponseBody>(json);
        }
    }
}
