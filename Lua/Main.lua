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
print("当前时间", TimeUtil.FmtTime(os.time()))
print("下一分钟", TimeUtil.FmtTime(TimeUtil.GetNextMinute()))
print("下一小时", TimeUtil.FmtTime(TimeUtil.GetNextHour()))
print("下一天", TimeUtil.FmtTime(TimeUtil.GetNextDay()))
print("下一周", TimeUtil.FmtTime(TimeUtil.GetNextWeek()))
print("下一月", TimeUtil.FmtTime(TimeUtil.GetNextMonth()))
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


local date1 = os.date("*t", os.time())
local date2 = os.date("*t", os.time({
    year = 2024,
    month = 6,
    day = 21,
    hour = 10,
    min = 1,
    sec = 1
}))
print(PrintUtil.FmtDateList({ date1, date2 }))