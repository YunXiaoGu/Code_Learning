---@class TimeUtil @时间工具类
local TimeUtil = {}

--- 格式化时间戳
---@param nTimestamp number @时间戳
---@return string @ Example: "2023-07-15 10:36:52"
function TimeUtil.FmtTime(nTimestamp)
    return os.date("%Y-%m-%d %H:%M:%S", nTimestamp)
end

--- 检查时间戳与日期的关系
---@param nTimestamp number @时间戳
---@param nYear number @年份
---@param nMonth number @月份
---@param nHour number @小时
---@param nMinute number @分钟
---@param nSecond number @秒
---@return number @ -1:时间戳早于指定日期，0:时间戳与指定日期相同, 1:时间戳晚于指定日期
function TimeUtil.CheckTime(nTimestamp, nYear, nMonth, nDay, nHour, nMinute, nSecond)
    -- 将指定日期转换为时间戳
    local nTargetTime = os.time({
        year  = nYear,
        month = nMonth,
        day   = nDay,
        hour  = nHour,
        min   = nMinute,
        sec   = nSecond
    })
    if nTimestamp < nTargetTime then
        return -1
    elseif nTimestamp > nTargetTime then
        return 1
    end
    return 0
end

--- 获取下一分钟的时间戳
---@return number
function TimeUtil.GetNextMinute()
    local tbDate = os.date("!*t")
    tbDate.sec = 0
    tbDate.min = tbDate.min + 1
    tbDate.isdst = false
    return os.time(tbDate)
end

--- 获取下一小时的时间戳
---@return number
function TimeUtil.GetNextHour()
    local tbDate = os.date("!*t")
    tbDate.sec = 0
    tbDate.min = 0
    tbDate.hour = tbDate.hour + 1
    tbDate.isdst = false
    return os.time(tbDate)
end

--- 将指定日期转换成时间戳
---@param nYear number @年份
---@param nMonth number|nil @月份
---@param nHour number|nil @小时
---@param nMinute number|nil @分钟
---@param nSecond number|nil @秒
---@return number
function TimeUtil.ConvertToTimestamp(nYear, nMonth, nDay, nHour, nMinute, nSecond)
    local dateTable = {
        year = nYear,
        month = nMonth or 1,
        day = nDay or 1,
        hour = nHour or 0,
        min = nMinute or 0,
        sec = nSecond or 0
    }
    return os.time(dateTable)
end

return TimeUtil
