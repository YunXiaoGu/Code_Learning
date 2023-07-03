require("Lua.preload")

-- 字符串分割
PrintUtil.PrintArray(StringUtil.Split("www.google.com", "."))
PrintUtil.PrintArray(StringUtil.Split("1+2-3+4-5+6--7+8-9=0", "-"))
PrintUtil.PrintArray(StringUtil.Split("......", "."))


-- 数字转成千分位格式
print(NumberUtil.AddComma(12345678.987654321))
print(NumberUtil.Truncate(0.1))
print(NumberUtil.Truncate(123.45600))
print(NumberUtil.Truncate(123.45600, 1))