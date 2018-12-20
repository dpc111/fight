/////////////////////////////////////////////////////
// do not modify the file, gen by const/gen.bat
/////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FightConst {
	//
	public const int CampGroupTypeSelf             = 1;             // 己方阵营
	public const int CampGroupTypeEnemy            = 2;             // 地方阵营
	public const int CampGroupTypeAll              = 3;             // 全部
	//
	public const int UnitTypeTower                 = 1;             // 塔
	public const int UnitTypeSoldier               = 2;             // 兵
	public const int UnitTypeBullet                = 3;             // 子弹
	public const int UnitTypeLive                  = 4;             // 塔 士兵
	public const int UnitTypeAll                   = 5;             // 所有
	//
	public const int SkillRangeTypeHitRange        = 1;             // 碰撞范围
	public const int SkillRangeTypeAttackRange     = 2;             // 攻击范围
	public const int SkillRangeTypeAll             = 3;             // 全局
	//
	public const int SkillTargetGroupNone          = 1;             // 无
	public const int SkillTargetGroupNearestOne    = 2;             // 最近元件
	public const int SkillTargetGroupLimitNum      = 3;             // 限定个数
	public const int SkillTargetGroupAll           = 4;             // 全局
	//
	public const int CampLeft                      = 1;             // 左方阵营
	public const int CampRight                     = 2;             // 右方阵营
	//
	public const int MoveTypeDir                   = 1;             // 直线
	public const int MoveTypeLock                  = 2;             // 锁定
	public const int MoveTypePath                  = 3;             // 按路线
	//
	public const int BulletHitCheckNone            = 1;             // 不检测
	public const int BulletHitCheckTarget          = 2;             // 检测目标
	public const int BulletHitCheckAll             = 3;             // 检测全部
	//
}