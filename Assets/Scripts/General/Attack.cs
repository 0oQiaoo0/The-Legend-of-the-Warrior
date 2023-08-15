using UnityEngine;

namespace General
{
    public class Attack : MonoBehaviour
    {
        public int damage;
        public float attackRange;
        public float attackRate;

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log(name + " attacked " + other.name);
            other.GetComponent<Character>()?.TakeDamage(this);
        }
    }
}
