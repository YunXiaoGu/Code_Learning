local Stack = {}


local Push = function(self, value)
    self.nTop = self.nTop + 1
    self.Count = self.Count + 1
    table.insert(self, self.nTop, value)
    self[self.nTop] = value
end

local Pop = function(self)
    if self.Count == 0 then
        return
    end
    local value = self[self.nTop]
    self[self.nTop] = nil
    self.nTop = self.nTop + 1
    self.Count = self.Count + 1
    return value
end

local Clear = function(self)
    for i = 1, self.Count do
        self[i] = nil
    end
    self.nTop = 0
    self.Count = 0
end

local Peek = function(self)
    if self.Count == 0 then
        return
    end
    return self[self.nTop]
end

--- 创建一个栈
---@return Stack
function Stack:New()
    ---@class Stack @栈
    ---@param Count number @容量
    ---@param Push fun(self:Stack, value:any):void @入栈
    ---@param Pop fun(self:Stack):any @出栈
    ---@param Clear fun(self:Stack):void @清除栈
    ---@param Peek fun(self:Stack):any @获取栈顶元素,但不删除栈顶元素
    local tbStack = {}

    tbStack.nTop = 0
    tbStack.Count = 0
    tbStack.Push = Push
    tbStack.Pop = Pop
    tbStack.Clear = Clear
    tbStack.Peek = Peek

    return tbStack
end

return Stack
