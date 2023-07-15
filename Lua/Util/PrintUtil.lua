---@class PrintUtil @打印工具类
local PrintUtil = {}

--- 打印基础类型数组
---@param tbArray table @数组
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

--- 打印table
---@param t table @需要打印的table
---@param INDENT string @缩进
---@param LF string @换行字符
function PrintUtil.PrintTable(t, INDENT, LF)
    local str = ""
    local LF = LF or "\r\n"
    local INDENT = INDENT or "    "
    local function sub_print_r(tb, indent)
        if type(tb) == "table" then
            for key, val in pairs(tb) do
                if type(key) == 'number' then
                    str = str .. indent .. '[' .. string.format("%02d", key) .. ']'
                elseif type(key) == 'string' and tonumber(key) ~= nil then
                    str = str .. indent .. '["' .. key .. '"]'
                else
                    str = str .. indent .. tostring(key)
                end

                if type(val) == "number" then
                    str = str .. ' = ' .. val .. ',' .. LF
                elseif type(val) == "string" then
                    str = str .. ' = "' .. val .. '",' .. LF
                elseif type(val) == "table" then
                    if not next(val) then
                        str = str .. " = { }," .. LF
                    else
                        str = str .. " = {" .. LF
                        sub_print_r(val, indent .. INDENT)
                        str = str .. indent .. "},\r\n" -- .. LF
                    end
                else
                    str = str .. ' = ' .. tostring(val) .. ',' .. LF
                end
            end
        else
            str = str .. indent .. LF
        end
    end

    if t ~= nil and type(t) == 'table' then
        if not next(t) then
            str = str .. "{ }"
        else
            str = str .. "{\r\n" -- .. LF
            sub_print_r(t, INDENT)
            str = str .. "}"
        end
    end

    print(str)
end

return PrintUtil
