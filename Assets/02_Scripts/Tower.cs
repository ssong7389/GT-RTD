using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Spine.Unity;
using Spine;
using Spine.Unity.AttachmentTools;

public class Tower : MonoBehaviour
{
    TowerManager tm;
    //GameManager gm;

    [SerializeField]
    public float dmg;
    [SerializeField]
    public float increment;

    // 2, 2.5 | 3.6, 4, 4.5 | 5.5, 6
    [SerializeField]
    public float range;

    // 공격 애니메이션 길이로
    [SerializeField]
    public float attackSpeed;
    [SerializeField]
    string towerName;
    public string TowerName
    {
        get { return towerName; }
    }
    public string assetName;
    // 스파인

    // 공격 이펙트
    // 타워 타입
    // 타워 등급
    [SerializeField]
    TowerManager.Ranks rank;
    [SerializeField]
    TowerManager.Type type;
    public TowerManager.Type Type
    {
        get { return type; }
    }
    public TowerManager.Ranks Rank
    {
        get { return rank; }
    }
    public enum Dir
    {
        back, front, side
    }
    Dir direction = Dir.front;
    public Dir Direction
    {
        get { return direction; }
        set
        {
            direction = value;
            SetSkeletonDirection();
            SetWeaponAtlas();
        }
    }
    public enum States
    {
        idle, attack
    }
    [SerializeField]
    States state;
    public States State
    {
        get { return state; }
        set
        {
            state = value;
            
            if (state == States.attack)
            {
                //skel.AnimationName = 
                //skel.loop = false;
                skel.AnimationName = $"{attackName}_{direction}";
                //skel.loop = false;
                //Debug.Log("att");
                //State = States.idle;
            }

            else
            {
                skel.AnimationName = $"{weaponType}_{state}_{direction}";
                skel.loop = true;
                //Debug.Log("idle");
            }
        }
    }
    public enum Weapon
    {
        // (1)  (1,2)       (1)    (2)      (1)    (1)    (2)    (1)
        rifle, gauntlet, twohand, basket, katana, staff, spear, sword, bow
    }
    [SerializeField]
    Weapon weaponType;
    Transform towerTr;
    [SerializeField]
    GameObject target;
    GameObject Target
    {
        get { return target; }
        set
        {
            target = value;
            if(target != null)
            {
                targetCtrl = Target.GetComponent<EnemyController>();
                State = States.attack;
            }
            else
            {
                State = States.idle;
            }
        }
    }
    EnemyController targetCtrl;
    SkeletonAnimation skel;
    string path;
    MeshRenderer meshRenderer;
    float originScaleXY;
    [SerializeField]
    string weaponAtlas;

    SpineAtlasAsset atlasAsset; 
    Atlas atlas;

    public string attackName;
    public string portraitName;
    public void InitTowerData(TowerManager.Ranks rank, TowerManager.Type type, float dmg, float increment,
        float range,float attackSpeed)
    {
        this.rank = rank;
        this.type = type;
        this.dmg = dmg;
        this.attackSpeed = attackSpeed;
        this.range = range;
        this.increment = increment;

        //Debug.Log(weaponAtlas);
    }
    public void InitTowerAssets(string towerName, string assetName, string weaponAtlas, string weaponType, string attackName, string portraitName)
    {
        this.towerName = towerName;
        this.assetName = assetName;
        this.weaponAtlas = weaponAtlas;
        this.weaponType = (Weapon)Enum.Parse(typeof(Weapon), weaponType);
        this.attackName = attackName;
        this.portraitName = portraitName;
    }
    private void Start()
    {
        tm = TowerManager.Instance;
        towerTr = GetComponent<Transform>();
        skel = GetComponent<SkeletonAnimation>();
        skel.loop = true;
        path = $"characters/{assetName}/{assetName}";
        atlasAsset = (SpineAtlasAsset)Resources.Load("items/items_Atlas");
        atlas = atlasAsset.GetAtlas();
        
        Direction = Dir.front;
        originScaleXY = towerTr.localScale.x;
        State = States.idle;

    }
    private void Update()
    {
        if (Target != null && targetCtrl.hp>0)
        {
            //Debug.Log(Quaternion.FromToRotation(Vector2.one, (target.transform.position - towerTr.position)).eulerAngles.z);

        }
        if (Target == null)
        {
            SetTargetAndAttack();
        }
    }
    void SetDirection()
    {
        float angle = Quaternion.FromToRotation((Target.transform.position - towerTr.position), new Vector2(-1, 1)).eulerAngles.z;
        if (angle > 0 && angle <= 90)
        {
            Direction = Dir.back;
        }
        else if (angle > 90 && angle <= 180)
        {
            Direction = Dir.side;
            towerTr.localScale = new Vector3(originScaleXY, originScaleXY);
        }
        else if (angle > 180 && angle <= 270)
        {
            Direction = Dir.front;
        }
        else if (angle > 270 && angle <= 360)
        {
            Direction = Dir.side;
            towerTr.localScale = new Vector3(-originScaleXY, originScaleXY, 1);
        }
    }
    public void InitTowerSkeleton()
    {
        skel = GetComponent<SkeletonAnimation>();
        path = $"characters/{assetName}/{assetName}";
        direction = Dir.front;
        SetSkeletonDirection();
    }
    void SetSkeletonDirection()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        //Debug.Log($"{path}_Material");
        meshRenderer.sharedMaterial = (Material)Resources.Load($"{path}_Material");
        meshRenderer.sortingOrder = -1;
        //Debug.Log($"{path}_{direction}_SkeletonData");
        SkeletonDataAsset newSkeletonData = (SkeletonDataAsset)Resources.Load($"{path}_{direction}_SkeletonData");
        if (skel.skeletonDataAsset != newSkeletonData)
        {
            skel.skeletonDataAsset = newSkeletonData;
            skel.Initialize(true);
            if (state == States.idle)
            {
                skel.AnimationName = $"{weaponType}_{state}_{direction}";
            }
            else if (state == States.attack)
            {
                skel.AnimationName = $"{attackName}_{direction}";
            }
        }
        //skel.AnimationName = $"{weaponType}_{state}_{direction}";
    }
    void SetWeaponAtlas()
    {
        Slot slot;
        switch (weaponType)
        {
            case Weapon.gauntlet:
                slot = skel.Skeleton.FindSlot($"[base]weapon1_{direction}");
                break;
            case Weapon.basket:
                slot = skel.Skeleton.FindSlot($"[base]weapon2_{direction}");
                break;
            case Weapon.spear:
                slot = skel.Skeleton.FindSlot($"[base]weapon2_{direction}");
                break;
            default:
                slot = skel.Skeleton.FindSlot($"[base]weapon1_{direction}");
                break;
        }
        Attachment originalAttachment = slot.Attachment;
        AtlasRegion region = atlas.FindRegion(weaponAtlas);
        
        float scale = skel.SkeletonDataAsset.scale;
        if (region == null)
        {
            slot.Attachment = null;
        }
        else if (originalAttachment != null)
        {
            slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);
        }
        else
        {
            var newRegionAttachment = region.ToRegionAttachment(region.name, scale);
            slot.Attachment = newRegionAttachment;
            
            //Bone muzzle = skel.Skeleton.FindBone("b.aim");
            //Vector3 pos = muzzle.GetLocalPosition();
            //GameObject go = new GameObject();
            //Instantiate(go);
            //go.transform.position = pos;
        }       
    }

    IEnumerator AttackCoroutine()
    {
        while (Target != null)
        {

            float distance = Vector2.Distance(towerTr.position, Target.transform.position);
            if (distance > range + 0.125)
            {
                Target.GetComponent<MeshRenderer>().material.color = Color.white;
                //Debug.Log(Vector2.Distance(towerTr.position, target.transform.position));
                Target = null;
                break;
            }
            else
            {
                if (Target.activeSelf)
                {
                    SetDirection();
                }
                else
                {
                    Target = null;
                    break;
                }
                if(State != States.attack)
                    State = States.attack;
                targetCtrl.hp -= dmg + increment * tm.GetUpgrade(type);

                yield return new WaitForSeconds(attackSpeed);
            }
        }
        
        //Debug.Log($"target null");
        yield return new WaitForSeconds(attackSpeed);
        StopCoroutine(AttackCoroutine());
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y +0.25f), range);
    }
    void SetTargetAndAttack()
    {
        if(Target == null)
        {
            Collider[] colls = Physics.OverlapSphere(transform.position + Vector3.up *0.25f, range);
            Dictionary<GameObject, float> nearby = new Dictionary<GameObject, float>();
            foreach (var coll in colls)
            {
                if (coll.CompareTag("ENEMY") || coll.CompareTag("BOSS"))
                {
                    nearby.Add(coll.gameObject, (coll.transform.position - towerTr.position).sqrMagnitude);
                }
            }

            if (nearby.Count == 0)
                return;

            nearby = nearby.OrderBy(near => near.Value).ToDictionary(near => near.Key, near => near.Value);

            GameObject nearest = nearby.First().Key;
            //Debug.Log($"min: {nearby.First().Value}");
            if (Vector2.Distance(towerTr.position, nearest.transform.position) < range)
            {
                Target = nearest;

                // 타겟 표시..
                Target.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(AttackCoroutine());
            }
        }
    }
}
