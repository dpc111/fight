/////////////////////////////////////////////////////
// do not modify the file, gen by const/gen.bat
/////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class GameDefine {
	//CampGroupType
	public const int CampGroupTypeSelf             = 1;             // 己方阵营
	public const int CampGroupTypeEnemy            = 2;             // 地方阵营
	public const int CampGroupTypeAll              = 3;             // 全部
	//UnitType
	public const int UnitTypeTower                 = 1;             // 塔
	public const int UnitTypeSoldier               = 2;             // 兵
	public const int UnitTypeBullet                = 3;             // 子弹
	public const int UnitTypeLive                  = 4;             // 塔 士兵
	public const int UnitTypeAll                   = 5;             // 所有
	//SkillRangeType
	public const int SkillRangeTypeHitRange        = 1;             // 碰撞范围
	public const int SkillRangeTypeAttackRange     = 2;             // 攻击范围
	public const int SkillRangeTypeAll             = 3;             // 全局
	//SkillTargetGroup
	public const int SkillTargetGroupNone          = 1;             // 无
	public const int SkillTargetGroupNearestOne    = 2;             // 最近元件
	public const int SkillTargetGroupLimitNum      = 3;             // 限定个数
	public const int SkillTargetGroupAll           = 4;             // 全局
	//Camp
	public const int CampNone                      = 0;             // 无
	public const int CampLeft                      = 1;             // 左方阵营
	public const int CampRight                     = 2;             // 右方阵营
	//MoveType
	public const int MoveTypeDir                   = 1;             // 直线
	public const int MoveTypeLock                  = 2;             // 锁定
	public const int MoveTypePath                  = 3;             // 按路线
	//BulletHitCheck
	public const int BulletHitCheckNone            = 1;             // 不检测
	public const int BulletHitCheckTarget          = 2;             // 检测目标
	public const int BulletHitCheckAll             = 3;             // 检测全部
	//SkillType
	public const int SkillTypeShoot                = 1;             // 射击
	public const int SkillTypeCreateSoldier        = 2;             // 造兵
	public const int SkillTypeAoe                  = 3;             // aoe
	//BulletType
	public const int BulletTypeLock                = 1;             // 锁定
	public const int BulletTypeDir                 = 2;             // 方向
	public const int BulletTypePath                = 3;             // 路径
	//BuffType
	public const int BuffTypeCac                   = 1;             // 
	//BuffUpdateType
	public const int BuffUpdateTypeOnce            = 1;             // 
	public const int BuffUpdateTypeLoop            = 2;             // 
	//BuffAddType
	public const int BuffAddTypeTime               = 1;             // 
	public const int BuffAddTypeLayer              = 2;             // 
	public const int BuffAddTypeResetTime          = 3;             // 
	//BuffRemoveType
	public const int BuffRemoveTypeAll             = 1;             // 
	public const int BuffRemoveTypeLayer           = 2;             // 
	//AttrType
	public const int AttrTypeNone                  = 0;             // 
	public const int AttrTypeHp                    = 1;             // 
	public const int AttrTypeArmor                 = 2;             // 
	public const int AttrTypeMoveSpeed             = 3;             // 
	public const int AttrTypeAttackCd              = 4;             // 
	public const int AttrTypeAttackRange           = 5;             // 
	public const int AttrTypeAttackDamage          = 6;             // 
	public const int AttrTypeAttackNum             = 7;             // 
	public const int AttrTypeNum                   = 8;             // 
	//UnitState
	public const int UnitStateNone                 = 1;             // 无
	public const int UnitStateBorn                 = 2;             // 出生
	public const int UnitStateIdle                 = 3;             // 闲着
	public const int UnitStateWalk                 = 4;             // 行进
	public const int UnitStateAttack               = 5;             // 攻击
	public const int UnitStateBeAttack             = 6;             // 受攻击
	public const int UnitStateDeath                = 7;             // 死亡
}