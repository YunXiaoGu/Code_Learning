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

-- 定义邮箱格式的正则表达式模式
local StrPattern = "[A-Za-z0-9$.%%%+%-]+@[A-Za-z0-9$.%%%+%-]+%.%w%w%w?%w?"
--- 判断字符串是否符合邮箱格式
---@param str string
---@return boolean
function StringUtil.IsEmail(str)
    local matchResult = string.match(str, StrPattern)
    if matchResult then
      return true
    else
      return false
    end
end

return StringUtil
