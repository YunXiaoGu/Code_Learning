---@class StringUtil @字符串工具类
local StringUtil = {}

--- 分割字符串
---@param str string @待分割字符串
---@param strDelimiter string @分割字符
---@return string[]
function StringUtil.Split(str, strDelimiter)
    if str == nil or str == "" or strDelimiter == nil then
        return {}
    end
    local nDelimiterLen = string.len(strDelimiter)
    local nStart = 1
    local tbArr = {}

    repeat
        local nPos = string.find(str, strDelimiter, nStart, true)
        if not nPos then
            break
        end
        table.insert(tbArr, string.sub(str, nStart, nPos - 1))
        nStart = nPos + nDelimiterLen
    until false

    table.insert(tbArr, string.sub(str, nStart))
    return tbArr
end

return StringUtil
