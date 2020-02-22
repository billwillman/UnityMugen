local setmetatable = setmetatable
local mugen = mugen or {}
local trigger = mugen.trigger
local math = math
if trigger ~= nil then
  return trigger
end

trigger = {}
mugen.trigger = trigger

require("Const")

local GlobaConfigMgr = MonoSingleton_GlobalConfigMgr.GetInstance()

function trigger:GetLuaCnsCfg(name)
	if name == nil then
		return nil
	end
	local luaCfg = GlobaConfigMgr:GetLuaCnsCfg(name)
	return luaCfg
end

function trigger:RegisterAnimationState(newState)
	if newState == nil then
		return nil
	end
	GlobaConfigMgr:RegisterAnimationState(newState)
end

-- 条件模块

function trigger:Abs(value)
  local ret = math.abs(value)
  return ret
end

function trigger:Floor(value)
  local ret = math.floor(value)
  return ret
end

-- 弧度制
function trigger:ACos(angle)
  local ret = math.acos(angle)
  return ret
end

math.randomseed(os.time())
function trigger:Random(num)
	return math.random(num)
end

function trigger:StateTypeSet(luaPlayer, stateType)
	if luaPlayer == nil or stateType == nil then
		return
	end
	local display = luaPlayer.PlayerDisplay;
   if display == nil then
	  return
   end
   display.StateType = stateType 
end

function trigger:PhysicsTypeSet(luaPlayer, stateType)
	if luaPlayer == nil or stateType == nil then
		return
	end
	local display = luaPlayer.PlayerDisplay;
   if display == nil then
	  return
   end
   display.PhysicsType = stateType 
end

function trigger:AnimElem(luaPlayer)
   if luaPlayer == nil then
	  return nil
   end
   local display = luaPlayer.PlayerDisplay;
   if display == nil then
	  return nil
   end
   local currentFrame = display.ImageCurrentFrame
   if currentFrame == nil then
	  return nil
   end
   return currentFrame + 1
end

function trigger:AnimElemTime(luaPlayer, frameNo)
	if luaPlayer == nil or frameNo == nil then
	  return nil
   end
   local display = luaPlayer.PlayerDisplay;
   if display == nil then
	  return nil
   end
   -- CNS里是从1开始算第一个图片帧
   frameNo = frameNo - 1
   local ret = display:AnimElemTime(frameNo)
   return ret
end

function trigger:AILevel(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.AILevel
end

function trigger:Alive(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	local ret = 0
	if attribe.IsAlive then
		ret = 1
	end
	return ret
end

function trigger:Anim(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display.Anim
	return ret
end

function trigger:AnimExist(luaPlayer, state)
	if luaPlayer == nil or state == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display:HasAniGroup(state)
	return ret
end

function trigger:Facing(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local isFlipX = display.IsFlipX
	if not isFlipX then
		return 1
	else
		return 0
	end
end

function trigger:HitCount(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.HitCount
end

function trigger:Life(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.Life
end

function trigger:Power(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.Power
end

function trigger:Statetype(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.StandType
end

function trigger:CanCtrl(luaPlayer)
	local ctrl = self:Ctrl(luaPlayer)
	if ctrl == nil then
		return false
	end
	return ctrl ~= 0
end

function trigger:Ctrl(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.Ctrl
end

function trigger:PlayAnim(luaPlayer, stateId, isLoop)
	if luaPlayer == nil or stateId == nil then
		return false
	end
	
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return false 
	end
	
	isLoop = isLoop or false
	local ret = display:PlayAni(stateId, isLoop, false)
	return ret
end

function trigger:PlayStandCns(luaPlayer)
	return self:PlayCnsByName(luaPlayer, "0", true)
end

function trigger:PlayCnsByName(luaPlayer, stateDefName, isLoop)
	if luaPlayer == nil or stateDefName == nil then
		return false
	end
	
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return false 
	end
	
	isLoop = isLoop or false
	local ret = display:PlayCnsAnimateByName(stateDefName, isLoop)
	return ret
end

function trigger:PlayCns(luaPlayer, stateDefId, isLoop)
	if luaPlayer == nil or stateDefId == nil then
		return false
	end
	
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return false 
	end
	
	isLoop = isLoop or false
	local ret = display:PlayCnsAnimate(stateDefId, isLoop)
	return ret
end

--[[
function trigger:ChangeState(luaPlayer, newState, isCns)
	if luaPlayer == nil or newState == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	isCns = isCns or true
	return display:ChangeState(newState, isCns)
end
--]]

function trigger:CtrlSet(luaPlayer, ctrl)
	return trigger:SetCtrl(luaPlayer, ctrl)
end

function trigger:SetCtrl(luaPlayer, ctrl)
	if luaPlayer == nil or ctrl == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	attribe.Ctrl = ctrl
end

function trigger:VelSet(luaPlayer, x, y)
	if luaPlayer == nil or (x == nil and y == nil) then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	if x ~= nil and y ~= nil then
		x = x / _cPerVelUnit
		y = -y / _cPerVelUnit
		display:SetVelSet(x, y)
	elseif x ~= nil then
		x = x / _cPerVelUnit
		display:SetVelSetX(x)
	elseif y ~= nil then
		y = -y / _cPerVelUnit
		display:SetVelSetY(y)
	end
	return true
end

function trigger:VelAdd(luaPlayer, x, y)
	if luaPlayer == nil or (x == nil and y == nil) then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	x = x or 0
	y = y or 0
	x = x / _cPerVelUnit
	y = -y / _cPerVelUnit
	display:VelAdd(x, y)
	return true
end

function trigger:PosY(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = -math.floor(display.PosY * _cPerUnit + 0.5)
	return ret
end

function trigger:PosX(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = math.floor(display.PosX * _cPerUnit + 0.5)
	return ret
end

function trigger:PosSet(luaPlayer, x, y)
	if luaPlayer == nil or (x == nil and y == nil) then
		--print("luaPlayer == nil or (x == nil and y == nil)")
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		--print("display == nil")
		return nil 
	end
	if x ~= nil then
		x = x / _cPerUnit
		display.PosX = x
	end
	if y ~= nil then
		y = -y / _cPerUnit
		display.PosY = y
	end
end

function trigger:GetPlayer(playerType)
	if playerType == nil then
		return nil
	end
	local ret = PlayerDisplay.GetPlayer(playerType)
	return ret
end

function trigger:Dist_X(luaPlayer, ohterPlayer)
	if luaPlayer == nil or ohterPlayer == nil then
		return nil
	end
	local s = luaPlayer.PlayerDisplay
	local o = ohterPlayer.PlayerDisplay
	if s == nil or o == nil then
		return nil
	end
	local ret = s:DistanceX(o)
	return ret
end

function trigger:IsPersistent(luaPlayer, state)
	if luaPlayer == nil or state == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay
	if display == nil then
		return nil
	end
	local ret = display:IsStatePersistent(state)
	return ret
end

function trigger:Persistent(luaPlayer, state, isPersistent)
	if luaPlayer == nil or state == nil or isPersistent then
		return
	end
	local display = luaPlayer.PlayerDisplay
	if display == nil then
		return
	end
	display:RegStatePersistent(luaPlayer, state, isPersistent)
end

function trigger:Dist_Y(luaPlayer, ohterPlayer)
	if luaPlayer == nil or ohterPlayer == nil then
		return nil
	end
	local s = luaPlayer.PlayerDisplay
	local o = ohterPlayer.PlayerDisplay
	if s == nil or o == nil then
		return nil
	end
	-- Unity和MUGEN的Y坐标相反
	local ret = -s:DistanceY(o)
	return ret
end

function trigger:DestroySelf(luaPlayer)
	if luaPlayer == nil then
		return
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil
	end
	return display:DestroySelf()
end

function trigger:CreateExplod(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil
	end
	local ret = display:CreateExplod()
	return ret
end

function trigger:PosAdd(luaPlayer, x, y)
	if luaPlayer == nil or (x == nil and y == nil) then
		return nil
	end
	x = x or 0
	y = y or 0
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	x = x / _cPerUnit
	y = -y / _cPerUnit
	display:PosAdd(x, y)
	return true
end

function trigger:VelX(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display.VelX * _cPerVelUnit
	return ret
end

function trigger:VelY(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = -display.VelY * _cPerVelUnit
	return ret
end

function trigger:VelMul(luaPlayer, x, y)
	if luaPlayer == nil or (x == nil and y == nil) then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	if x == nil then
		x = 1
	else
		x = x / _cPerVelUnit
	end
	if y == nil then
		y = 1
	else
		y = -y / _cPerVelUnit
	end
	display:VelMul(x, y)
	return true;
end

function trigger:Time(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret  = display:Trigger_Time()
	return ret
end

function trigger:AnimTime(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display:Trigger_AnimTime()
	return ret
end

function trigger:Stateno(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display.Stateno
	return ret
end

function trigger:PrevStateNo(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display.PrevStateNo
	return ret
end

function trigger:Pause(luaPlayer, time)
	if luaPlayer == nil or time == nil then
		return
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return
	end
	display:CtlPause(time)
end

function trigger:Var(luaPlayer, index)
	if luaPlayer == nil or index == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	local isOk, value = attribe:GetIntVars(index, nil)
	if not isOk then
		return nil
	end
	return value
end

function trigger:VarSet(luaPlayer, index, value)
	if luaPlayer == nil or index == nil or value == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	attribe:SetIntVars(index, value)
end

function trigger:fVar(luaPlayer, index)
	if luaPlayer == nil or index == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	local isOk, value = attribe:GetFloatVars(index, nil)
	if not isOk then
		return nil
	end
	return value
end

function trigger:fVarSet(luaPlayer, index, value)
	if luaPlayer == nil or index == nil or value == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	attribe:SetFloatVars(index, value)
end

function trigger:Command(luaPlayer, cmdName)
	if luaPlayer == nil or cmdName == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display:IsCommandInputKeyOk(cmdName)
	return ret
end

function trigger:SwitchPallet(luaPlayer, idx)
	if luaPlayer == nil or idx == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display:SwitchPallet(idx)
	return ret
end

function trigger:RemoveExplod(luaPlayer, id)
	if luaPlayer == nil or id == nil then
		return
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return 
	end
	display:RemoveExplod(id)
end

-- 处理模块

function trigger:PlaySnd(luaPlayer, group, index)
	self:Do_PlaySnd(luaPlayer, group, index)
end

function trigger:Do_PlaySnd(luaPlayer, group, index)
	if luaPlayer == nil or group == nil or index == nil then
		return
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return 
	end
	display:PlaySound(group, index)
end

function trigger:Do_StatePlaySnd(luaPlayer, state)
	if luaPlayer == nil or state == nil then
		return
	end
	self:Do_PlaySnd(luaPlayer, state.value1, state.value2)
end

-- 创建飞行道具
function trigger:CreateProj(luaPlayer, isCreateHelper)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil
	end
	isCreateHelper = isCreateHelper or false
	local proj, helper = display:CreateProjectile(isCreateHelper)
	return proj, helper
end

-- 帮助模块
function trigger:Help_CreateStateDef(luaCfg, name)
	if luaCfg == nil or name == nil then
		return nil
	end
	local id = luaCfg:CreateStateDef(name)
	return id
end

function trigger:Help_GetStateDef(luaCfg, id)
	if luaCfg == nil or id == nil then
		return nil
	end
	local def = luaCfg:GetStateDef(id)
	return def;
end

function trigger:Help_InitLuaPlayer(newLuaPlayer, basePlayer)
	if newLuaPlayer == nil or basePlayer == nil then
		return
	end
	
	local org = basePlayer.Data
	if org == nil then
		return
	end
	
	local display = newLuaPlayer.PlayerDisplay;
	if display == nil then
		return 
	end
	
	local dst = display.Attribe
	if dst == nil then
		return
	end
	
	if org.life ~= nil then
		dst.life = org.life
	end
	
	if org.attack ~= nil then
		dst.attack = org.attack
	end
	
	if org.defence ~= nil then
		dst.defence = org.defence
	end
	
	if org.fall ~= nil and org.fall.defence_up ~= nil then
		dst.fail__defence_up = org.fall.defence_up
	end
	
	if org.liedown ~= nil and org.liedown.time ~= nil then
		dst.liedown__time = org.liedown.time
	end
	
	if org.airjuggle ~= nil then
		dst.airjuggle = org.airjuggle
	end
	
	if org.sparkno ~= nil then
		dst.sparkno = org.sparkno
	end
	
	if org.guard ~= nil and org.guard.sparkno ~= nil then
		dst.guard__sparkno = org.guard.sparkno
	end
	
	if org.KO ~= nil and org.KO.echo ~= nil then
		dst.ko__echo = org.KO.echo
	end
	
	if org.Power ~= nil then
		dst.Power = org.Power
	end
	
	if org.volume ~= nil then
		dst.volume = org.volume
	end
	
	if org.IntPersistIndex ~= nil then
		dst.IntPersistIndex = org.IntPersistIndex
	end
	
	if org.FloatPersistIndex ~= nil then
		dst.FloatPersistIndex = org.FloatPersistIndex
	end
	-- 初始化變量
	dst:InitVars()
	dst:ResetDatas()
end

return trigger
