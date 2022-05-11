using System.Collections.Generic;
using UnityEngine;
using Shared;
using PokerKing.UI;
using PokerKing.Utility;

namespace PokerKing.Gameplay
{
    public class PokerKing_ChipSpawner : MonoBehaviour
    {
        public static PokerKing_ChipSpawner Instance;
        [SerializeField] GameObject[] chips;
        [SerializeField] Transform[] spawnPostions;
        [SerializeField] Transform chipHolder;

        Dictionary<Chip, GameObject> chipContainer =
            new Dictionary<Chip, GameObject>();


        int chipOrderInLayer = 10;

        private void Awake()
        {
            Instance = this;
        }
        public void Start()
        {
            chipContainer.Add(Chip.Chip2, chips[6]);
            chipContainer.Add(Chip.Chip10, chips[0]);
            chipContainer.Add(Chip.Chip50, chips[1]);
            chipContainer.Add(Chip.Chip100, chips[2]);
            chipContainer.Add(Chip.Chip500, chips[3]);
            chipContainer.Add(Chip.Chip1000, chips[4]);
            chipContainer.Add(Chip.Chip5000, chips[5]);
            PokerKing_Timer.Instance.onTimeUp += () => chipOrderInLayer = 10;
        }
        public GameObject Spawn(int positinIndex, Chip chipType, Transform parent)
        {
            var chip = Instantiate(chipContainer[chipType], parent);
            //chip.GetComponent<SpriteRenderer>().sortingOrder = chipOrderInLayer++;
            chip.SetActive(true);
            chip.transform.position = spawnPostions[positinIndex].position;
            // StartCoroutine(WOF_UiHandler.Instance.StartServer_Animation());
            return chip;
        }
    }
}
