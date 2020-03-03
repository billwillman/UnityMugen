local trigger = require("trigger")
local _InitCommonCns = require("commonCns")

local setmetatable = setmetatable
local GlobaConfigMgr = MonoSingleton_GlobalConfigMgr.GetInstance()

local kfm720 = {}
kfm720.__index = kfm720

function kfm720:new()
   -- 静态数据
   if self._isInit == nil then
		self._isInit = true
		self:_initData()
		self:_initSize()
		self:_initMovement()
		self:_initStateDefs()
		self:_initCmds()
    end
   -- 动态数据
   local t = {PlayerDisplay = nil}
   return setmetatable(t, kfm720)
end

--====================外部调用接口==============================


function kfm720:OnInit(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	trigger:Help_InitLuaPlayer(self, self)
	_InitCommonCns(self)
end

function kfm720:OnDestroy()
  self.PlayerDisplay = nil
end

function kfm720:OnGetAICommandName(cmdName)
	
end

--===========================================================

function kfm720:_initData()
  if self.Data ~= nil then
	return
  end
  self.Data = {};
  
  self.Data.life = 1000
  self.Data.attack = 100
  self.Data.defence = 100
  
  self.Data.fall = {}
  self.Data.fall.defence_up = 50
  
  self.Data.liedown = {}
  self.Data.liedown.time = 60
  
  self.Data.airjuggle = 15
  self.Data.sparkno = 2
  
  self.Data.guard = {}
  self.Data.guard.sparkno = 40
  
  self.Data.KO = {}
  self.Data.KO.echo = 0
  
  self.Data.volume = 0
  self.Data.IntPersistIndex = 60
  self.Data.FloatPersistIndex = 40
end

function kfm720:_initSize()
  if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end

function kfm720:_initMovement()
	if self.Movement ~= nil then
		return
	end
	
	self.movement = {}
	self.movement.yaccel = 1.76

	self.velocity = {}
	self.velocity.run = {}
	self.velocity.run.fwd = Vector2.New(18.4, 0)
	self.velocity.run.back = Vector2.New(-18,-15.2)

	self.velocity.airjump = {}
	self.velocity.airjump.neu = Vector2.New(0,-32.4)
	self.velocity.airjump.back = Vector2.New(-10.2,0)
	self.velocity.airjump.fwd = Vector2.New(10,0)
	self.velocity.airjump.y = -27.5
end

--=====================================创建StateDef===================================
function kfm720:_initStateDefs()
	local luaCfg = GlobaConfigMgr:GetLuaCnsCfg("kfm720")
	if luaCfg == nil then
		return
	end
	
	-- 创建各种状态
	self:_initStateDef_3000(luaCfg)
end

function kfm720:_initStateDef_3000(luaCfg)
	local id = trigger:Help_CreateStateDef(luaCfg, "3000")
	local def = trigger:Help_GetStateDef(luaCfg, id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Juggle = 4
	def.Animate = 3000
	def.Ctrl = 0
	def.Sprpriority = 2
	def.Velset_x = 0
	def.Velset_y = 0
end
--======================================================================================

function kfm720:_initCommands(luaCfg)
	local cmd = luaCfg:CreateCmd("a")
	cmd.time = 1
	cmd:AttachKeyCommands("a")
	
	cmd = luaCfg:CreateCmd("b")
	cmd.time = 1
	cmd:AttachKeyCommands("b")
	
	cmd = luaCfg:CreateCmd("c")
	cmd.time = 1
	cmd:AttachKeyCommands("c")
	
	cmd = luaCfg:CreateCmd("x")
	cmd.time = 1
	cmd:AttachKeyCommands("x")
	
	cmd = luaCfg:CreateCmd("y")
	cmd.time = 1
	cmd:AttachKeyCommands("y")
	
	cmd = luaCfg:CreateCmd("z")
	cmd.time = 1
	cmd:AttachKeyCommands("z")
	
	cmd = luaCfg:CreateCmd("holdfwd")
	cmd.time = 1
	cmd:AttachKeyCommands("/$F")
	
	cmd = luaCfg:CreateCmd("holdback")
	cmd.time = 1
	cmd:AttachKeyCommands("/$B")
	
	cmd = luaCfg:CreateCmd("holdup")
	cmd.time = 1
	cmd:AttachKeyCommands("/$U")
	
	cmd = luaCfg:CreateCmd("holddown")
	cmd.time = 1
	cmd:AttachKeyCommands("/$D")
	
	cmd = luaCfg:CreateCmd("start")
	cmd.time = 1
	cmd:AttachKeyCommands("s")
	
	cmd = luaCfg:CreateCmd("FF")
	cmd.time = 10
	cmd:AttachKeyCommands("F, F")
	
	cmd = luaCfg:CreateCmd("BB")
	cmd.time = 10
	cmd:AttachKeyCommands("B, B")
	
	cmd = luaCfg:CreateCmd("QCF_y")
	cmd:AttachKeyCommands("~D, DF, F, y")
end

function kfm720:On_Taunt(cmdName)
	local triggerall = trigger:Command(self, "start")
	local trigger1 = trigger:Statetype(self) ~= Mugen.Cns_Type.A and trigger:CanCtrl(self)
	local ret = triggerall and trigger1
	return ret
end

--[[
function kfm720:On_Run_Fwd(cmdName)
	local trigger1 = trigger:Command(self, "FF") and 
						trigger:Statetype(self) == Mugen.Cns_Type.S and 
						trigger:CanCtrl(self)
	
	return trigger1 
end
--]]

function kfm720:On_Run_Back(cmdName)
	local trigger1 = trigger:Command(self, "BB") and 
						trigger:Statetype(self) == Mugen.Cns_Type.S and 
						trigger:CanCtrl(self)
	return trigger1 
end

function kfm720:On_QCF_y(cmdName)
	local triggerall = trigger:Command(self, "QCF_y")
	-- 暂时忽略COMBO条件
	return triggerall
end

function kfm720:On_Stand_Light_Punch(cmdName)
	local triggerall = trigger:Command(self, "x") and (not trigger:Command(self, "holddown"))
	local trigger1 = trigger:Statetype(self) == Mugen.Cns_Type.S and trigger:CanCtrl(self)
	local trigger2 = trigger:Stateno(self) == 200 and trigger:Time(self) > 6
	local ret = triggerall and (trigger1 or trigger2)
	return ret
end

function kfm720:On_Stand_Light_Kick(cmdName)
	local triggerall = trigger:Command(self, "a") and (not trigger:Command(self, "holddown"))
	local trigger1 = trigger:Statetype(self) == Mugen.Cns_Type.S and trigger:CanCtrl(self)
	local trigger2 = trigger:Stateno(self) == 200 and trigger:Time(self) > 7
	local trigger3 = trigger:Stateno(self) == 230 and trigger:Time(self) > 9
	local ret = triggerall and (trigger1 or trigger2 or trigger3)
	return ret
end

function kfm720:On_Standing_Strong_Kick(cmdName)
	local triggerall = trigger:Command(self, "b") and (not trigger:Command(self, "holddown"))
	local trigger1 = trigger:Statetype(self) == Mugen.Cns_Type.S and trigger:CanCtrl(self)
	local trigger2 = trigger:Stateno(self) == 200 and trigger:Time(self) > 5
	local trigger3 = trigger:Stateno(self) == 230 and trigger:Time(self) > 6
	local ret = triggerall and (trigger1 or trigger2 or trigger3)
	return ret
end

function kfm720:_initState_Default(luaCfg)
	local aiCmd = luaCfg:CreateAICmd("Taunt")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "195"
	aiCmd.OnTriggerEvent = self.On_Taunt
	--[[
	aiCmd = luaCfg:CreateAICmd("Run Fwd", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "100"
	aiCmd.AniLoop = true
	aiCmd.OnTriggerEvent = self.On_Run_Fwd
	--]]
	
	aiCmd = luaCfg:CreateAICmd("Run Back")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "105"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_Run_Back
	
	aiCmd = luaCfg:CreateAICmd("Strong Kung Fu Palm")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "1010"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_QCF_y

	aiCmd = luaCfg:CreateAICmd("Stand Light Punch")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "200"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_Stand_Light_Punch

	aiCmd = luaCfg:CreateAICmd("Stand Light Kick")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "230"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_Stand_Light_Kick

	aiCmd = luaCfg:CreateAICmd("Standing Strong Kick")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "240"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_Standing_Strong_Kick
end

function kfm720:initCmd_1050(luaCfg)

--------------------------- register StateDef 1050 ---------------------------
		local id = luaCfg:CreateStateDef("1050")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.S

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 4

		def.PowerAdd = 55

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 2

		def.Animate = 1050

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 1)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 0, 2, false)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

						if trigger1 then

								trigger:PosAdd(luaPlayer, 80, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayCnsByName(luaPlayer, "1051", false)

						end

				end


end

function kfm720:initCmd_1051(luaCfg)

--------------------------- register StateDef 1051 ---------------------------
		local id = luaCfg:CreateStateDef("1051")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.A

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 0

		def.PowerAdd = 0

		def.Velset_x = 8/_cPerVelUnit

		def.Velset_y = 24/_cPerVelUnit

		def.Sprpriority = 0

		def.IsHitdefPersist = true

		def.Animate = 1051

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						trigger:VelAdd(luaPlayer, nil, 1.8)

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:VelY(luaPlayer) > 0) and (trigger:PosY(luaPlayer) >= -40))

						if trigger1 then

								trigger:PlayCnsByName(luaPlayer, "1052", false)

						end

				end


end



function kfm720:initCmd_1052(luaCfg)

--------------------------- register StateDef 1052 ---------------------------
		local id = luaCfg:CreateStateDef("1052")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.S

		def.MoveType = Mugen.Cns_MoveType.I

		def.Juggle = 0

		def.PowerAdd = 0

		def.Velset_x = 0

		def.Velset_y = 0

		def.Sprpriority = 1

		def.Animate = 1052

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:PosSet(luaPlayer, nil, 0)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 40, 0, false)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 3)

						if trigger1 then

								trigger:CtrlSet(luaPlayer, 1)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

						if trigger1 then

								trigger:PosAdd(luaPlayer, -60, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end


end


function kfm720:initCmd_FeiMaoTui(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("飞毛腿")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 飞毛腿 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("飞毛腿")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "1050"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "飞毛腿"))
								and (trigger:Ctrl(luaPlayer) == 1)
								and (trigger:Statetype(luaPlayer) == Mugen.Cns_Type.S)
						return triggle1
				end

end

function kfm720:initCmd_1055(luaCfg)

--------------------------- register StateDef 1055 ---------------------------
		local id = luaCfg:CreateStateDef("1055")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.A

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 0

		def.PowerAdd = 0

		def.Ctrl = 0

		def.Sprpriority = 0

		def.Animate = 1055

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 100, 0, false)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 0, 1, false)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:PosAdd(luaPlayer, 40, -40)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:VelAdd(luaPlayer, nil, -24)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:VelY(luaPlayer) > 0) and (trigger:PosY(luaPlayer) >= -20))

						if trigger1 then

								trigger:PlayCnsByName(luaPlayer, "1056", false)

						end

				end



end


function kfm720:initCmd_1056(luaCfg)

--------------------------- register StateDef 1056 ---------------------------
		local id = luaCfg:CreateStateDef("1056")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.S

		def.MoveType = Mugen.Cns_MoveType.I

		def.Juggle = 0

		def.PowerAdd = 0

		def.Velset_x = 0/_cPerVelUnit

		def.Velset_y = 0/_cPerVelUnit

		def.Sprpriority = 1

		def.Animate = 1056

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:PosSet(luaPlayer, nil, 0)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:Time(luaPlayer) == 0)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 40, 0, false)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end



end




function kfm720:initCmd_FeiMaoTui2(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("飞毛腿2")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 飞毛腿2 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("飞毛腿2")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "1055"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "飞毛腿2"))
								and (trigger:Ctrl(luaPlayer) == 1)
								and (trigger:Statetype(luaPlayer) == Mugen.Cns_Type.S)
						return triggle1
				end

end


function kfm720:_initCmds()
	local luaCfg = trigger:GetLuaCnsCfg("kfm720")
	if luaCfg == nil then
		return
	end
	
	self:_initCommands(luaCfg)
	self:_initState_Default(luaCfg)
	self:_initStateDef(luaCfg)
	self:initCmd_1050(luaCfg)
	self:initCmd_1051(luaCfg)
	self:initCmd_1052(luaCfg)
	self:initCmd_FeiMaoTui(luaCfg)
	self:initCmd_1055(luaCfg)
	self:initCmd_1056(luaCfg)
	self:initCmd_FeiMaoTui2(luaCfg)
end

function kfm720:_initStateDef(luaCfg)
--------------------- start -------------------
	local id = luaCfg:CreateStateDef("195")
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.Ctrl = 0
	def.Animate = 195
	def.Velset_x = 0
	def.Velset_y = 0
	def.MoveType = Mugen.Cns_MoveType.I
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Sprpriority = 2
	
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime, Mugen.CnsStateType.none)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local aniTime = trigger:AnimTime(luaPlayer)
			if aniTime == 0 then
				trigger:PlayStandCns(luaPlayer)
			end
		end
---------------------- Strong Kung Fu Palm ------------------
	id = luaCfg:CreateStateDef("1010")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Juggle = 4
	def.PowerAdd = 60
	def.Velset_x = 0
	def.Velset_y = 0
	def.Animate = 1010
	def.Ctrl = 0
	def.Sprpriority = 2
	-- [State 1010, 1]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
		  local t = trigger:Time(luaPlayer)
		  if t == 9 then
			trigger:PlaySnd(luaPlayer, 0, 3)
		  end
		end
	-- [State 1010, 2]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local frame = trigger:AnimElem(luaPlayer)
			if frame == 2 then
				trigger:PosAdd(luaPlayer, 80, 0)
			end
		end
	-- [State 1010, 3]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local frame = trigger:AnimElem(luaPlayer)
			if frame == 3 or frame == 13 then
				trigger:PosAdd(luaPlayer, 40, 0)
			end
		end
	-- [State 1010, 4]
	-- [State 1010, 5]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local frame = trigger:AnimElem(luaPlayer)
			if frame == 5 then
				trigger:PosAdd(luaPlayer, 20, 0)
				trigger:VelSet(luaPlayer, 16, 0)
			end
		end
	-- [State 1010, 8]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local aniTime = trigger:AnimTime(luaPlayer)
			if aniTime == 0 then
				trigger:SetCtrl(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end
----------------- RUN_BACK --------------
	id = luaCfg:CreateStateDef("105")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.A
	def.MoveType = Mugen.Cns_MoveType.A
	def.Ctrl = 0
	def.Animate = 105
	def.Sprpriority = 1
-- [State 105, 1]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			--print(tt)
			if tt == 0 then
				print("State 105, 1")
				trigger:VelSet(luaPlayer, -18, -15.2)
			end
		end
-- [State 105, 2]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			if tt == 2 then
				print("State 105, 2")
				trigger:CtrlSet(luaPlayer, 1)
			end
		end
-- [State 105, 3]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:VelY(luaPlayer) > 0 and trigger:PosY(luaPlayer) >= 0
			if trigger1 then
				print("State 105, 3")
				trigger:PlayCnsByName(luaPlayer, "106")
			end
		end

	id = luaCfg:CreateStateDef("106")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Ctrl = 0
	def.Animate = 47
-- [State 106, 1]
-- [State 106, 2]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			print("106 Check")
			local trigger1 = trigger:Time(luaPlayer) == 0
			if trigger1 then
				trigger:VelSet(luaPlayer, nil, 0)
				trigger:PosSet(luaPlayer, nil, 0)
			end
		end
-- [State 106, 4]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 7
			if trigger1 then
				trigger:SetCtrl(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end
------------------------- Run Fwd -----------------
--[[
	id = luaCfg:CreateStateDef("100")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Animate = 100
	def.Sprpriority = 1
-- State 100, 1
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			trigger:VelSet(luaPlayer, 18.4, nil)
		end
--]]
------------------------ Stand Light Punch -------------
	id = luaCfg:CreateStateDef("200")
  	def = luaCfg:GetStateDef(id)
  	def.Type = Mugen.Cns_Type.S
  	def.MoveType = Mugen.Cns_MoveType.A
  	def.PhysicsType = Mugen.Cns_PhysicsType.S
  	def.Juggle = 1
  	def.Velset_x = 0
  	def.Velset_y = 0
  	def.Ctrl = 0
  	def.Animate = 200
  	def.PowerAdd = 10
  	def.Sprpriority = 2
-- State 200, 2
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 1
			if trigger1 then
				trigger:PlaySnd(luaPlayer, 0, 0)
			end
		end
-- State 200, 3
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:AnimTime(luaPlayer) == 0
			if trigger1 then
				trigger:SetCtrl(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end
------------------------- Stand Light Kick -----------------------
	id = luaCfg:CreateStateDef("230")
  	def = luaCfg:GetStateDef(id)
  	def.Type = Mugen.Cns_Type.S
  	def.MoveType = Mugen.Cns_MoveType.A
  	def.PhysicsType = Mugen.Cns_PhysicsType.S
  	def.Juggle = 4
  	def.PowerAdd = 11
  	def.Ctrl = 0
  	def.Velset_x = 0
  	def.Velset_y = 0
  	def.Animate = 230
  	def.Sprpriority = 2
 -- State 230, 1
 	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
 	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 2
			if trigger1 then
				trigger:PlaySnd(luaPlayer, 0, 1)
			end
		end
-- State 230, 3
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
 	state.OnTriggerEvent =
 		function (luaPlayer, state)
 			local trigger1 = trigger:AnimTime(luaPlayer) == 0
 			if trigger1 then
 				trigger:PlayStandCns(luaPlayer)
 			end
 		end
 --------------------- Standing strong kick ------------------------
 	id = luaCfg:CreateStateDef("240")
  	def = luaCfg:GetStateDef(id)
  	def.Type = Mugen.Cns_Type.S
  	def.MoveType = Mugen.Cns_MoveType.A
  	def.PhysicsType = Mugen.Cns_PhysicsType.S
  	def.Juggle = 5
  	def.PowerAdd = 30
  	def.Ctrl = 0
  	def.Velset_x = 0
  	def.Velset_y = 0
  	def.Animate = 240
  	def.Sprpriority = 2
 -- State 240, 1
 	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
 	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 2
			if trigger1 then
				trigger:PlaySnd(luaPlayer, 0, 1)
			end
		end
-- state 240, 3
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
 	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:AnimElem(luaPlayer) == 7
			if trigger1 then
				trigger:PosAdd(luaPlayer, 48, nil)
			end
		end
-- State 240, 4
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
 	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:AnimTime(luaPlayer) == 0
			if trigger1 then
				trigger:SetCtrl(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end
end

setmetatable(kfm720, {__call = kfm720.new})
return kfm720