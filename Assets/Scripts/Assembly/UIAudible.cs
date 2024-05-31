using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UIAudible : MonoBehaviour, IObserver
{
    [SerializeField] public AudioClip PickleRick;
    [SerializeField] public AudioClip DontHateThePlayer;
    [SerializeField] public AudioClip EvilMortySpeech;
    [SerializeField] public AudioClip Gagablag;
    [SerializeField] public AudioClip GetSchwifty;
    [SerializeField] public AudioClip HeadBentOver;
    [SerializeField] public AudioClip IDoScience;
    [SerializeField] public AudioClip IJustWannaDie;
    [SerializeField] public AudioClip ImMrMeeseek;
    [SerializeField] public AudioClip OhGeez;
    [SerializeField] public AudioClip OooWee;
    [SerializeField] public AudioClip RickAndMortySpeech;
    [SerializeField] public AudioClip TinyRick;
    [SerializeField] public AudioClip ShowMeWhatYouGot;
    [SerializeField] public AudioClip Wabbalubbadubdub;
    [SerializeField] public AudioClip WhatsMyPurpose;
    [SerializeField] public AudioClip BothPlayersWin;

    private Dictionary<string, AudioClip> SoundsEffectForCharacters = new();

    private Dictionary<string, AudioClip> SoundOfGameOver = new();
    public AudioSource SoundEffect;
    public AudioSource BackgroundMusic;

    private void OnEnable()
    {
        // GameManager.gameManager.AddObserver(this);
    }

    private void OnDisable()
    {
        GameManager.gameManager.RemoveObserver(this);
    }

    void Start()
    {
        GameManager.gameManager.AddObserver(this);

        SoundsEffectForCharacters.Add("Rick Sánchez", Wabbalubbadubdub);
        SoundsEffectForCharacters.Add("Morty Smith", OhGeez);
        SoundsEffectForCharacters.Add("Butter Robot", WhatsMyPurpose);
        SoundsEffectForCharacters.Add("Pickle Rick", PickleRick);
        SoundsEffectForCharacters.Add("Tiny Rick", TinyRick);
        SoundsEffectForCharacters.Add("Cromulons", ShowMeWhatYouGot);
        SoundsEffectForCharacters.Add("Meeseek’s Army", IJustWannaDie);
        SoundsEffectForCharacters.Add("Cromulon’s Concert", HeadBentOver);
        SoundsEffectForCharacters.Add("Mr. Poopybutthole", OooWee);
        SoundsEffectForCharacters.Add("Mr. Meeseeks", ImMrMeeseek);

        SoundOfGameOver.Add("Vórtice del Caos", EvilMortySpeech);
        SoundOfGameOver.Add("Smiths Interdimensionales", RickAndMortySpeech);
        SoundOfGameOver.Add("Rickpública", DontHateThePlayer);
        SoundOfGameOver.Add("Unidad Cósmica", GetSchwifty);
        SoundOfGameOver.Add("Both", BothPlayersWin);

    }

    void Update()
    {

    }

    public void OnNotify(System.Enum action, Card card)
    {
        UnityEngine.Debug.Log($"UIAudible ha sido notificado para {action.ToString()}");

        switch (action)
        {
            case GameEvents.Summon:
                if (card != null)
                {
                    if (SoundsEffectForCharacters.ContainsKey(card.Name))
                        PlaySoundEffect(SoundsEffectForCharacters[card.Name]);
                }
                return;

            case GameEvents.FinishGame:
                if (GameManager.Player1.GamesWon == 2 && GameManager.Player2.GamesWon < 2)
                {
                    BackgroundMusic.clip = SoundOfGameOver[GameManager.Player1.PlayerFaction];
                    SoundEffect.Stop();
                    BackgroundMusic.Play();
                    BackgroundMusic.loop = false;
                }

                if (GameManager.Player2.GamesWon == 2 && GameManager.Player1.GamesWon < 2)
                {
                    BackgroundMusic.clip = SoundOfGameOver[GameManager.Player2.PlayerFaction];
                    BackgroundMusic.Play();
                    SoundEffect.Stop();
                    BackgroundMusic.loop = false;
                }

                if (GameManager.Player1.GamesWon == 2 && GameManager.Player2.GamesWon == 2)
                {
                    BackgroundMusic.clip = SoundOfGameOver["Both"];
                    BackgroundMusic.Play();
                    SoundEffect.Stop();
                    BackgroundMusic.loop = false;
                }
                return;

            case GameEvents.Invoke:
                if (card != null)
                {
                    if (SoundsEffectForCharacters.ContainsKey(card.Name))
                        PlaySoundEffect(SoundsEffectForCharacters[card.Name]);
                }
                return;

        }
    }


    public void PlaySoundEffect(AudioClip soundClip)
    {
        // Disminuir el volumen de la música de fondo
        BackgroundMusic.volume = 0.3f; // Ajusta este valor según sea necesario

        // Reproducir el clip con PlayOneShot
        SoundEffect.PlayOneShot(soundClip);

        // Restaurar el volumen de la música de fondo después de que el clip haya terminado
        StartCoroutine(RestoreVolumeAfterDelay(soundClip.length));
    }

    IEnumerator RestoreVolumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        BackgroundMusic.volume = 1f; // Restaurar el volumen original
    }
}
