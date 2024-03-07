// Copyright (C) 2015 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;

// This class represents the music button that is used in several places in the demo.
// It handles the logic to enable and disable the demo's music and store the player
// selection to PlayerPrefs.
public class MusicButton : MonoBehaviour
{
    private SpriteSwapper m_spriteSwapper;
    private bool m_on;

    private void Start()
    {
        m_spriteSwapper = GetComponent<SpriteSwapper>();
        m_on = PlayerPrefs.GetInt("music_on") == 1;
        if (!m_on)
            m_spriteSwapper.SwapSprite();
    }

    public void Toggle()
    {
        m_on = !m_on;
        PlayerPrefs.SetInt("music_on", m_on ? 1 : 0);
        var backgroundAudioSource = GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>();
        // backgroundAudioSource.volume = m_on ? .5f : 0;
        if (m_on) {
            Debug.Log("Onnnnnnnnnnnnnnnnnnnnnn");
            backgroundAudioSource.FadeIn();
        } else {
            Debug.Log("Offffffffffffffffffffff");
            backgroundAudioSource.FadeOut();
        }
    }

    public void ToggleSprite()
    {
        m_on = !m_on;
        m_spriteSwapper.SwapSprite();
    }
}