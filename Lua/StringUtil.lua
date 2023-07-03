---@class StringUtil @字符串工具类
local StringUtil = {}

--- 分割字符串
---@param str string @待分割字符串
---@param strDelimiter string @分割字符
---@param bIgnoreEmptyStr boolean  @默认为false，对于分割后出现的空字符串，不放入结果中
---@return string[] @分割结果
function StringUtil.Split(str, strDelimiter, bIgnoreEmptyStr)
    if str == nil or str == "" or strDelimiter == nil then
        return {}
    end
    local nDelimiterLen = string.len(strDelimiter)
    local nStart = 1
    local tbArr = {}

    local strTmp
    repeat
        local nPos = string.find(str, strDelimiter, nStart, true)
        if not nPos then
            break
        end
        strTmp = string.sub(str, nStart, nPos - 1)
        if bIgnoreEmptyStr == nil or bIgnoreEmptyStr then
            if strTmp ~= "" then
                table.insert(tbArr, strTmp)
            end
        else
            table.insert(tbArr, strTmp)
        end
        nStart = nPos + nDelimiterLen
    until false

    table.insert(tbArr, string.sub(str, nStart))
    return tbArr
end

return StringUtil
