---@class TimeUtil @时间工具类
local TimeUtil = {}

--- 一分钟秒数
local OneMinuteSeconds = 60
--- 一小时秒数
local OneHourSeconds = 3600
--- 一天秒数
local OneDaySeconds = 86400
--- 每月的天数
local DaysPerMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }
--- 每月的天数（闰年）
local DaysPerMonthLeapYear = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }

--- 格式化时间戳
---@param nTimestamp number @时间戳
---@return string @ Example: "2023-07-15 10:36:52"
function TimeUtil.FmtTime(nTimestamp)
    return os.date("%Y-%m-%d %H:%M:%S", nTimestamp)
end

--- 格式化日期
---@param tDate osdate @日期
---@return string @ Example: "2023-07-15 10:36:52"
function TimeUtil.FmtDate(tDate)
    return os.date("%Y-%m-%d %H:%M:%S", tDate.year, tDate.month, tDate.day, tDate.hour, tDate.min, tDate.sec)
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

--- 将指定日期转换成时间戳
---@param nYear number @年份
---@param nMonth number|nil @月份，nil则
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

--- 将秒数转换为天、小时、分钟和秒
---@param nSeconds number @秒数
---@return number, number, number, number
function TimeUtil.ParseSeconds(nSeconds)
    if nSeconds < 0 then
        return 0, 0, 0, 0
    end
    local days = math.floor(nSeconds / OneDaySeconds)
    local hours = math.floor((nSeconds % OneDaySeconds) / OneHourSeconds)
    local minutes = math.floor((nSeconds % OneHourSeconds) / 60)
    local seconds = nSeconds % 60
    return days, hours, minutes, seconds
end

--- 获取下一分钟开始的时间戳
---@return number
function TimeUtil.GetNextMinute()
    local nMinute = math.floor(os.time() / 60)
    return (nMinute + 1) * 60
end

--[[
--- 获取下一分钟开始的时间戳
---@return number
function TimeUtil.GetNextMinute()
    -- 获取当前时间的信息
    local date = os.date("*t", os.time())

    -- 将秒数归零
    date.sec = 0

    -- 分钟+1
    if date.min + 1 < 60 then
        date.min = date.min + 1
    else
        date.min = 0
        -- 小时+1
        if date.hour < 24 then
            date.hour = date.hour + 1
        else
            date.hour = 0
            -- 天数+1
            local nTotalDays = IsLeapYear(date.year) and Days2[date.month] or Days[date.month]
            if date.day + 1 <= nTotalDays then
                date.day = date.day + 1
            else
                date.day = 1
                -- 月份+1
                if date.month + 1 <= 12 then
                    date.month = date.month + 1
                else
                    date.month = 1
                    date.year = date.year + 1
                end
            end
        end
    end
    return os.time(date)
end
]]

--- 获取下一个小时开始的时间戳
---@return number
function TimeUtil.GetNextHour()
    local nHour = math.floor(os.time() / 3600)
    return (nHour + 1) * 3600
end

--- 获取下一天开始的时间戳
---@return number
function TimeUtil.GetNextDay()
    local nDay = math.floor(os.time() / 86400)
    return (nDay + 1) * 86400
end

--- 获取下一天开始的时间戳
---@return number
function TimeUtil.GetNextDay()
    local date = os.date("*t", os.time())
    -- 设置分钟和秒为0，小时和日期加1
    date.min = 0
    date.sec = 0
    date.hour = 0
    date.day = date.day + 1
    -- 处理日期越界的情况，os.time()会自动处理
    -- date = os.date("*t", os.time(date))
    return os.time(date)
end

--- 获取下一周开始的时间戳
---@return number
function TimeUtil.GetNextWeek()
    local time = os.time()
    local date = os.date("*t", time)

    -- 第几天
    local nIndex = (date.wday == 1) and 7 or date.wday - 1
    
    -- 计算下个周一的时间戳
    -- local nLeftDay = nIndex == 0 and 0 or 7 - nIndex
    -- local date2 = os.date("*t", time + (nLeftDay * OneDaySeconds))
    -- date2.hour = 0
    -- date2.min = 0
    -- date2.sec = 0
    -- return os.time(date2)

    date.day = date.day + (8 - nIndex)
    date.hour = 0
    date.min = 0
    date.sec = 0
    return os.time(date)
end

--- 获取下一月开始的时间戳
---@return number
function TimeUtil.GetNextMonth()
    local now = os.time()
    local date = os.date("*t", now)
    if date.month + 1 < 12 then
        date.month = date.month + 1
    else
        date.month = 1
        date.year = date.year + 1
    end
    date.day = 1
    date.hour = 0
    date.min = 0
    date.sec = 0
    return os.time(date)
end

return TimeUtil
