using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.UI
{
    public class BuildingSvContent : MonoBehaviour, IBuildingSvContent
    {

        public int ColumnCount; // Number of columns to have on the scroll view.
        public float HorizontalGap; // Horizontal gap between the buttons
        public float VerticalGap; // Vertical gap between the buttons
        private float UpperTresholdYValue => _placementYPosition + 0.5f * ButtonHeight; // Upper treshold value to check if any button crossed.
        private float _lowerTresholdYValue; // Lower treshold value to check if any button crossed.
        private float ButtonWidth => (ContentFrameworkSize.x - (ColumnCount + 1) * HorizontalGap) / ColumnCount; // Button width
        private float ButtonHeight => ButtonWidth; // Button height
        private float PlacementXPosition => transform.position.x - ContentFrameworkSize.x / 2 + HorizontalGap + ButtonWidth / 2; // X coordinate for the placement position for the first button.
        private float _placementYPosition; // Y coordinate for the placement position for the first button.
        private int RowCount => Mathf.CeilToInt(ProduciblesList.Count / ColumnCount) * 11; // Number of rows in the scroll view.
        public Vector2 ContentFrameworkSize => GetComponent<RectTransform>().rect.size; // Size of the content framework which will contain the buttons.
        public List<GameObject> ProduciblesList; // List contains every producible building's button.
        public List<GameObject> AllButtonsList; // List of buttons in content framework

        private void Start()
        {
            AllButtonsList = new List<GameObject>();
            Populate();
        }

        public void Update()
        {
            var oneButtonFromTop = AllButtonsList.OrderByDescending(x => x.transform.position.y).First(); // Button at top
            var oneButtonFromBottom = AllButtonsList.OrderBy(x => x.transform.position.y).First(); // Button at bottom

            if (oneButtonFromTop.transform.position.y > UpperTresholdYValue) // Carry the top buttons to bottom if scrolled down.
            {
                var buttonsToCarry = AllButtonsList.Where(x => x.transform.position.y > UpperTresholdYValue);

                foreach (var button in buttonsToCarry)
                {
                    button.transform.position = new Vector3(button.transform.position.x, oneButtonFromBottom.transform.position.y - ButtonHeight - VerticalGap, button.transform.position.z);
                }
            }
            else if (oneButtonFromBottom.transform.position.y < _lowerTresholdYValue) // Carry the bottom buttons to top if scrolled up.
            {
                var buttonsToCarry = AllButtonsList.Where(x => x.transform.position.y < _lowerTresholdYValue);

                foreach (var button in buttonsToCarry)
                {
                    button.transform.position = new Vector3(button.transform.position.x, oneButtonFromTop.transform.position.y + ButtonHeight + VerticalGap, button.transform.position.z);
                }
            }
           
        }

        /*
         Populate function creates buttons to fill the scroll view content.
         */
        public void Populate()
        {
            _placementYPosition = transform.position.y + ContentFrameworkSize.y / 2 + 1.5f * ButtonHeight + 2f * VerticalGap; // First button's y coordinate is one and a half ButtonHeight and two VerticalGaps above the Framework's top position.

            var producibleIndex = 0; // The last row may not contain buttons as much as number of columns. This index prevents ProduciblesList to fall into OutOfIndex error.

            for (var rowIndex = 1; rowIndex <= RowCount; rowIndex++) // For each row...
            {
                for (var columnIndex = 1; columnIndex <= ColumnCount; columnIndex++) // For each column...
                {
                    if (producibleIndex >= ProduciblesList.Count * 11) continue;
                    var newButton = Instantiate(ProduciblesList[producibleIndex % ProduciblesList.Count], transform); // New button to add to the content.
                    newButton.transform.position = new Vector2(PlacementXPosition + (columnIndex - 1) * (ButtonWidth + HorizontalGap), _placementYPosition - (rowIndex - 1) * (ButtonHeight + VerticalGap)); // Reposition the new button to the appropriate position.
                    AllButtonsList.Add(newButton);
                    producibleIndex++;
                }
            }

            _lowerTresholdYValue = (_placementYPosition - (RowCount) * (ButtonHeight + VerticalGap)) - 0.5f * ButtonHeight; // Place the lowerTresholdYValue below the last button.
        }

    }
}