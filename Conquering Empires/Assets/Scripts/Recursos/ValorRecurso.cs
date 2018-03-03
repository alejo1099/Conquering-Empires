using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValorRecurso : MonoBehaviour {

	public int CantidadRecurso { get; set; }

	private void OnDisabled()
	{
		CantidadRecurso = 0;
	}
}
