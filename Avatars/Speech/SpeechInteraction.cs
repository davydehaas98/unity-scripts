using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

using Avatars.Enum;
using DialogFlow;

namespace Avatars.Speech
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class SpeechInteraction : MonoBehaviour, IMixedRealityInputHandler
    {
        [Header("Input")]
        [SerializeField] private MixedRealityInputAction action = MixedRealityInputAction.None;

        [Header("Voice")]
        [SerializeField] private Gender gender = Gender.Neutral;

        [Header("Indicator")]
        [SerializeField] private GameObject indicator = null;
        [SerializeField] private Material listeningMaterial = null;
        [SerializeField] private Material defaultMaterial = null;
        [SerializeField] private Material busyMaterial = null;

        private bool _isMicrophoneConnected = false;
        private bool _isMicrophoneRecording = false;
        private bool _isProcessingAudio = false;
        private bool _isPressingAction = false;
        private bool _inProximity = false;

        private DialogFlowService _service = null;
        private AudioSource _source = null;
        private int _minimumFrequency = 0;
        private int _maximumFrequency = 0;

        private Transform playerTransform = null;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            
            if (_source == null)
            {
                Debug.LogWarning("No AudioSource Component Provided!");
            }
            
            if (Microphone.devices.Length <= 0)
            {
                Debug.LogWarning("No Microphone Connected!");
                return;
            }

            Microphone.GetDeviceCaps(null, out _minimumFrequency, out _maximumFrequency);
            
            if (_minimumFrequency == 0 && _maximumFrequency == 0)
            {
                _maximumFrequency = 44100;
            }

            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
            indicator.gameObject.SetActive(false);
            
            _service = new DialogFlowService();
            _isMicrophoneConnected = true;
        }

        private void Update()
        {
            if (_isMicrophoneConnected && _inProximity)
            {
                if (_isPressingAction)
                {
                    StartListening();
                }
                else
                {
                    StopListening();
                }
                
                SetMaterial();
            }

            if (_inProximity && playerTransform)
            {
                transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            playerTransform = other.transform;
            _inProximity = true;
            indicator.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            playerTransform = null;
            _inProximity = false;
            indicator.SetActive(false);
        }

        public void OnInputUp(InputEventData eventData)
        {
            if (!_isMicrophoneConnected || !_inProximity || eventData.MixedRealityInputAction != action)
            {
                return;
            }

            _isPressingAction = false;
            eventData.Use();
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (!_isMicrophoneConnected || !_inProximity || eventData.MixedRealityInputAction != action)
            {
                return;
            }

            _isPressingAction = true;
            eventData.Use();
        }

        private void StartSpeaking(AudioClip audio)
        {
            if (audio == null)
            {
                _isProcessingAudio = false;
                
                return;
            }

            _source.spatialBlend = 0.0f;
            _source.clip = audio;
            _source.Play();
            _isProcessingAudio = false;
        }

        private void StartListening()
        {
            if (_isMicrophoneRecording || _isProcessingAudio || _source.isPlaying)
            {
                return;
            }

            _source.clip = Microphone.Start(null, true, 20, 16000);
            _isMicrophoneRecording = true;
        }

        private void StopListening()
        {
            if (!_isMicrophoneRecording || _isProcessingAudio || _source.isPlaying)
            {
                return;
            }

            Microphone.End(null);
            _isMicrophoneRecording = false;
            _isProcessingAudio = true;

            StartCoroutine(_service.DetectIntent(_source.clip, gender, StartSpeaking));
        }

        private void SetMaterial()
        {
            if (_isMicrophoneRecording)
            {
                indicator.GetComponent<Renderer>().material = listeningMaterial;
                
                return;
            }
            
            if (_isProcessingAudio || _source.isPlaying)
            {
                indicator.GetComponent<Renderer>().material = busyMaterial;
                
                return;
            }
            
            indicator.GetComponent<Renderer>().material = defaultMaterial;
        }
    }
}
