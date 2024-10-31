using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioDescricao
{
    public enum TIPO_VOZ { Masculina, Feminina }

    [SerializeField] private AudioClip _descricaoVozMasculina;
    [SerializeField] private AudioClip _descricaoVozFeminina;

    public AudioClip AudioMasculino => _descricaoVozMasculina;
    public AudioClip AudioFeminino => _descricaoVozFeminina;
}
