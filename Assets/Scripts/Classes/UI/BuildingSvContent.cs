using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Classes
{
    public class BuildingSvContent : MonoBehaviour, IBuildingSvContent
    {

        public int ColumnCount;
        public float HorizontalGap;
        public float VerticalGap;
        private float UpperTresholdYValue => PlacementYPosition + 0.5f * ButtonHeight;
        private float LowerTresholdYValue;
        private float ButtonWidth => (ContentFrameworkSize.x - (ColumnCount + 1) * HorizontalGap) / ColumnCount;
        private float ButtonHeight => ButtonWidth;
        private float PlacementXPosition => transform.position.x - ContentFrameworkSize.x / 2 + HorizontalGap + ButtonWidth / 2;
        private float PlacementYPosition;
        private int RowCount => Mathf.CeilToInt(ProduciblesList.Count * 11 / ColumnCount);
        public Vector2 ContentFrameworkSize => GetComponent<RectTransform>().rect.size;
        public List<GameObject> ProduciblesList;
        public List<GameObject> AllButtonsList;

        void Start()
        {
            AllButtonsList = new List<GameObject>();
            Populate();
        }

        public void Populate()
        {
            PlacementYPosition = transform.position.y + ContentFrameworkSize.y / 2 + 1.5f * ButtonHeight + 2f * VerticalGap;

                var producibleIndex = 0;

                for (int rowIndex = 1; rowIndex <= RowCount; rowIndex++)
                {
                    for (int columnIndex = 1; columnIndex <= ColumnCount; columnIndex++)
                    {
                        if (producibleIndex < ProduciblesList.Count * 11)
                        {
                            var newButton = Instantiate(ProduciblesList[producibleIndex % ProduciblesList.Count], transform);
                            newButton.transform.position = new Vector2(PlacementXPosition + (columnIndex - 1) * (ButtonWidth + HorizontalGap), PlacementYPosition - (rowIndex - 1) * (ButtonHeight + VerticalGap));
                            AllButtonsList.Add(newButton);
                            producibleIndex++;
                        }
                    }
                }

            LowerTresholdYValue = (PlacementYPosition - (RowCount) * (ButtonHeight + VerticalGap)) - 0.5f * ButtonHeight;
        }

        public void Update()
        {
            var oneButtonFromTop = AllButtonsList.OrderByDescending(x => x.transform.position.y).First();
            var oneButtonFromBottom = AllButtonsList.OrderBy(x => x.transform.position.y).First();

            if (oneButtonFromTop.transform.position.y > UpperTresholdYValue) // Carry the top buttons to bottom
            {
                var buttonsToCarry = AllButtonsList.Where(x => x.transform.position.y > UpperTresholdYValue);

                foreach (var button in buttonsToCarry)
                {
                    button.transform.position = new Vector3(button.transform.position.x, oneButtonFromBottom.transform.position.y - ButtonHeight - VerticalGap, button.transform.position.z);
                }
            }
            else if (oneButtonFromBottom.transform.position.y < LowerTresholdYValue) // Carry the bottom buttons to top
            {
                var buttonsToCarry = AllButtonsList.Where(x => x.transform.position.y < LowerTresholdYValue);

                foreach (var button in buttonsToCarry)
                {
                    button.transform.position = new Vector3(button.transform.position.x, oneButtonFromTop.transform.position.y + ButtonHeight + VerticalGap, button.transform.position.z);
                }
            }
           
        }
    }
}