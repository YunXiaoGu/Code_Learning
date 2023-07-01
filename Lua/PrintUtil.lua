---@class PrintUtil @打印工具类
local PrintUtil = {}

--- 打印基础类型数组
---@param tbArray ValueType[] @数组
---@return string
function PrintUtil.PrintArray(tbArray)
    if tbArray == nil then
        return
    end
    local str = "{ "
    local nLen = #tbArray
    for i, value in ipairs(tbArray) do
        str = str .. value .. (i < nLen and ", " or "")
    end
    str = str .. " }"
    print(str)
end

return PrintUtil
