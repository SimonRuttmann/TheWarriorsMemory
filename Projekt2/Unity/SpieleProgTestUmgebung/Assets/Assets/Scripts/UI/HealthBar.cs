using System.Collections.Generic;
using System.Linq;
using Scripts.Enums;
using Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        private GameObject _fillColor;
        private Team _team;
        private void Start()
        {
            InitializeChildren();
            SetTeamColor();
        }

        private void InitializeChildren()
        {
            IList<GameObject> children = new List<GameObject>();
                
            for (var i = 0; i < transform.childCount; i++) 
                children.Add(transform.GetChild(i).gameObject);
                
            _fillColor = children.First(child => child.name.EqualsIgnoreCase("FillColor"));
        }

        private void SetTeamColor()
        {
            var image = _fillColor.GetComponent<Image>();

        
            image.color = _team.Equals(Team.Enemy) ? Color.blue : Color.green;
        }
        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
        }

        public void SetTeam(Team yourTeam)
        {
            _team = yourTeam;
        }
        public void SetHealth(int health)
        {
            slider.value = health;
        }
    }
}
