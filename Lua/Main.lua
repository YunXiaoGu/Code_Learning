print("------------------------------------------------------------------------- 导入模块")
StringUtil = require("Lua.Util.StringUtil")
PrintUtil = require("Lua.Util.PrintUtil")
NumberUtil = require("Lua.Util.NumberUtil")
TimeUtil = require("Lua.Util.TimeUtil")
Stack = require("Lua.Collections.Stack")
Queue = require("Lua.Collections.Queue")

print("\n------------------------------------------------------------------------- 字符串工具")
PrintUtil.PrintArray(StringUtil.Split("www.google.com", "."))
PrintUtil.PrintArray(StringUtil.Split("1+2-3+4-5+6--7+8-9=0", "-"))
PrintUtil.PrintArray(StringUtil.Split("......", "."))

-- 测试邮箱验证函数
local str1 = "123@example.com"
local str2 = ".@com"
local str3 = "A1230+-@test.org"
print("是否是邮箱", str1, StringUtil.IsEmail(str1))
print("是否是邮箱", str2, StringUtil.IsEmail(str2))
print("是否是邮箱", str3, StringUtil.IsEmail(str3))

print("\n------------------------------------------------------------------------- 数值工具")
print(NumberUtil.AddComma(12345678.987654321))
print(NumberUtil.Truncate(0.1))
print(NumberUtil.Truncate(123.45600))
print(NumberUtil.Truncate(123.45600, 1))


print("\n------------------------------------------------------------------------- Lua集合")
local stack = Stack:New()
stack:Push("1")
stack:Push("3")
stack:Push(3)
stack:Push("4")
stack:Push(false)
stack:Push({})
stack:Push(function() end)
stack:Push()

print(stack:ToString())


local queue = Queue:New()
queue:Enqueue(1)
queue:Enqueue(2)
queue:Enqueue("3")
queue:Enqueue(false)
queue:Enqueue(true)
queue:Enqueue()
queue:Enqueue({})
queue:Enqueue(function() end)
print(queue:ToString())

print("\n------------------------------------------------------------------------- 时间工具")
-- 当前时间
print(TimeUtil.FmtTime(os.time()))
-- 下一分钟
print(TimeUtil.FmtTime(TimeUtil.GetNextMinute()))
-- 下一小时
print(TimeUtil.FmtTime(TimeUtil.GetNextHour()))
-- 判断时间戳
local nTimestamp = os.time()
local nTargetTime = os.time({
    year  = 2023,
    month = 7,
    day   = 15,
    hour  = 12,
    min   = 0,
    sec   = 0
})
local nResult = TimeUtil.CheckTime(nTimestamp, 2023, 7, 15, 12, 0, 0)
print("当前日期:" .. TimeUtil.FmtTime(nTimestamp), "指定日期:" .. TimeUtil.FmtTime(nTargetTime), "是否相等", nResult)
