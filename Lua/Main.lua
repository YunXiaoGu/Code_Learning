require("Lua.preload")

-- ----------------------------------------------------- 字符串分割
-- PrintUtil.PrintArray(StringUtil.Split("www.google.com", "."))
-- PrintUtil.PrintArray(StringUtil.Split("1+2-3+4-5+6--7+8-9=0", "-"))
-- PrintUtil.PrintArray(StringUtil.Split("......", "."))


-- ----------------------------------------------------- 数字转成千分位格式
-- print(NumberUtil.AddComma(12345678.987654321))
-- print(NumberUtil.Truncate(0.1))
-- print(NumberUtil.Truncate(123.45600))
-- print(NumberUtil.Truncate(123.45600, 1))


-- ----------------------------------------------------- Lua 集合
local stack = Stack:New()
local stack2 = Stack:New()
stack:Push("1")
stack:Push("2")
stack:Push("3")
stack2:Push("3")

PrintUtil.PrintTable(stack)
print(stack[2])
