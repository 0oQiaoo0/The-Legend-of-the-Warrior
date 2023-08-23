using SaveLoad;
using SO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utilities;

namespace General
{
    public class Character : MonoBehaviour, ISavable
    {
        [Header("事件监听")]
        public VoidEventSO newGameEvent;
        [Header("基本属性")]
        public float maxHealth;
        public float currentHealth;
        public float maxPower;
        public float currentPower;
        public float powerRecoverSpeed;
        [Header("受伤无敌")]
        public float invulnerableDuration;
        [HideInInspector] public float invulnerableCounter;
        public bool invulnerable;

        public UnityEvent<Character> onHealthChange;
        public UnityEvent<Character> onPowerChange;
        public UnityEvent<Transform> onTakeDamage;
        public UnityEvent onDie;
        private void OnEnable()
        {
            newGameEvent.OnEventRaised += NewGame;
            ISavable savable = this;
            savable.RegisterSaveData();
        }
        private void OnDisable()
        {
            newGameEvent.OnEventRaised -= NewGame;
            ISavable savable = this;
            savable.UnRegisterSaveData();
        }
        private void Start()
        {
            //玩家初始状态重置&生成场景时敌人状态重置
            currentPower = maxPower;
            currentHealth = maxHealth;
        }
        private void NewGame()
        {
            //重新开始游戏时玩家状态重置
            currentPower = maxPower;
            currentHealth = maxHealth;
            //OnHealthChange?.Invoke(this);//或许需要再搞个事件
            //OnPowerChange?.Invoke(this);
        }
        private void Update()
        {
            if (invulnerable)
            {
                invulnerableCounter -= Time.deltaTime;
                if (invulnerableCounter <= 0)
                {
                    invulnerable = false;
                }
            }
            RecoverPower();
        }

        private void RecoverPower()
        {
            if (currentPower + Time.deltaTime * powerRecoverSpeed >= maxPower) currentPower = maxPower;
            else currentPower += Time.deltaTime * powerRecoverSpeed;

            onPowerChange?.Invoke(this);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Water")) return;
            
            if (currentHealth == 0) return;
            
            //死亡、更新血量
            onDie?.Invoke();
            currentHealth = 0;
            onHealthChange?.Invoke(this);
        }

        public void TakeDamage(Attack attacker)
        {
            if (!invulnerable)
            {
                if (currentHealth <= attacker.damage)
                {
                    currentHealth = 0;
                    //触发死亡
                    onDie?.Invoke();
                }
                else
                {
                    currentHealth -= attacker.damage;
                    TriggerInvulnerable();
                    //执行受伤
                    onTakeDamage?.Invoke(attacker.transform);
                }
                onHealthChange?.Invoke(this);
            }
        }
        /// <summary>
        /// 触发受伤无敌
        /// </summary>
        private void TriggerInvulnerable()
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }

        public void PowerSpend(float cost)
        {
            currentPower -= cost;
            onPowerChange?.Invoke(this);
        }

        public DataDefinition GetDataID()
        {
            return GetComponent<DataDefinition>();
        }

        public void GetSaveData(Data data)
        {
            data.CharacterPosDict[GetDataID().ID] = new SerializableVector3(transform.position);
            data.FloatSavedData[GetDataID().ID + "health"] = currentHealth;
            data.FloatSavedData[GetDataID().ID + "power"] = currentPower;
        }

        public void LoadData(Data data)
        {
            if (data.CharacterPosDict.TryGetValue(GetDataID().ID, out var value))
            {
                transform.position = value.ToVector3();
            }
            if (data.FloatSavedData.TryGetValue(GetDataID().ID + "health", out var health))
            {
                currentHealth = health;
            }
            if (data.FloatSavedData.TryGetValue(GetDataID().ID + "power", out var power))
            {
                currentPower = power;
            }
            onHealthChange?.Invoke(this);
        }
    }
}
