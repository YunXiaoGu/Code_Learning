local Queue = {}

---@param self Queue
local Enqueue = function(self, value)
    self.nLast = self.nLast + 1
    self.Count = self.Count + 1
    self[self.nLast] = value
end

---@param self Queue
local Dequeue = function(self)
    if self.Count == 0 then
        return
    end
    local value = self[self.nFirst]
    self[self.nFirst] = nil
    self.nFirst = self.nFirst - 1
    self.Count = self.Count - 1
    return value
end

---@param self Queue
local Clear = function(self)
    if self.Count == 0 then
        return
    end
    for i = 1, self.Count do
        self[i] = nil
    end
    self.nFirst = 0
    self.nLast = 0
    self.Count = 0
end

---@param self Queue
local Peek = function(self)
    if self.Count == 0 then
        return
    end
    return self[self.nLast]
end

---@param self Queue
local ToString = function(self)
    if self.Count == 0 then
        return "{ }"
    end
    local str = "{ "
    local nCount = self.Count
    local value, strType
    for i = 1, nCount do
        value = self[i]
        strType = type(value)
        if strType == "string" then
            str = str .. "\"" .. value .. "\""
        else
            str = str .. tostring(value)
        end
        str = str .. (i < nCount and ", " or "")
    end
    str = str .. " }"
    return str
end

--- 创建一个队列
---@return Queue
function Queue:New()
    ---@class Queue @队列
    ---@param Count number @容量
    ---@param Enqueue fun(self:Queue, value:any):void @进队
    ---@param Dequeue fun(self:Queue):any @出队
    ---@param Clear fun(self:Queue):void @清除队列
    ---@param Peek fun(self:Queue):any @获取队首元素,但不删除队首元素
    local tbQueue = {}

    tbQueue.nFirst = 0
    tbQueue.nLast = 0
    tbQueue.Count = 0
    tbQueue.Enqueue = Enqueue
    tbQueue.Dequeue = Dequeue
    tbQueue.Clear = Clear
    tbQueue.Peek = Peek
    tbQueue.ToString = ToString

    return tbQueue
end

return Queue
