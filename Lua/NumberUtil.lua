---@class NumberUtil @数值工具类
local NumberUtil = {}

local MathAbs = math.abs
local StringSub = string.sub
local StringFind = string.find
local StringFormat = string.format
local StringLen = string.len

--- 移除末尾的0
--- 0.10  ->  "0.1"
-- 0.10100 ->  "0.101"
---@param nNumber number|string @数字，如：0.10、"0.100"、"0.0010"
---@return string @去除末尾0的数字
function NumberUtil.RemoveZeros(nNumber)
    if nNumber == nil then
        return ""
    end
    if nNumber == 0 then
        return "0"
    end
    local strNumber = tostring(nNumber)
    local i = StringLen(strNumber)
    local strResult = StringSub(strNumber, i, i)
    while strResult == "0" do
        i = i - 1
        strResult = StringSub(strNumber, i, i)
    end
    if StringSub(strNumber, i, i) == "." then
        i = i - 1
    end
    return StringSub(strNumber, 1, i)
end

--- 截取到小数点后几位，不会四舍五入
--- 0.12345 ->  0.1/0.12/0.123/0.1234
---@param nNumber number @要截取的数字
---@param nPrecision number @小数点后几位，默认为0，即将小数全部截取掉
---@return string @截取后的数字
function NumberUtil.Truncate(nNumber, nPrecision)
    if nNumber == nil then
        return ""
    end
    if nNumber == 0 then
        return "0"
    end
    nPrecision = nPrecision or 0
    local strNumber = tostring(nNumber)
    -- 小数点位置
    local nDecimalPos = StringFind(strNumber, ".", 1, true)
    if nPrecision == 0 then
        return StringSub(strNumber, 1, nDecimalPos - 1)
    end
    if nDecimalPos and (nDecimalPos + nPrecision <= #strNumber) then
        return StringSub(strNumber, 1, nDecimalPos + nPrecision)
    end
    return strNumber
end

--- 数字格式化成带有千分位","
---@param nNumber number @要截取的数字
---@return string @格式化后的数字
function NumberUtil.AddComma(nNumber)
    if nNumber == nil then
        return ""
    end
    if nNumber == 0 then
        return "0"
    end

    --小数点及右边的不变，小数点左边的每3位加","
    local strNumber = tostring(nNumber)
    local nLen = StringLen(strNumber)
    --没必要加[,]号
    if nLen <= 3 then
        return strNumber
    end

    local str = ""
    local nEndPos = 1
    local nStartPos = nLen
    local nDotPos = StringFind(strNumber, "[.]")
    if nDotPos then
        if nDotPos <= 4 then
            -- 有小数点的情况下, 整数部分长度 + 小数点长度1 <= 4 没必要加[,]号
            return strNumber
        end
        str = StringSub(strNumber, nDotPos)
        nStartPos = nDotPos - 1
    end
    for nIdx = nStartPos, nEndPos, -3 do
        str = StringSub(strNumber, math.max(1, nIdx - 2), nIdx) .. (nIdx == nStartPos and "" or ",") .. str
    end
    return str
end

return NumberUtil
