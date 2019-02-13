local setmetatable = setmetatable

local kfm720 = {}
kfm720.__index = kfm720

function kfm720:new()
   -- 静态数据
   self:_initData()
   self:_initSize()
   self:_initStateDefs()
   -- 动态数据
   local t = {PlayerDisplay = nil}
   return setmetatable(t, kfm720)
end

function kfm720:OnDestroy()
  self.PlayerDisplay = nil
end

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
end

function kfm720:_initSize()
  if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end

function kfm720:_initStateDefs()
end

setmetatable(kfm720, {__call = kfm720.new})
return kfm720