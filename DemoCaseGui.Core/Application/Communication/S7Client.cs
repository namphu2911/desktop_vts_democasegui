﻿using S7.Net;
using Timer = System.Timers.Timer;

namespace DemoCaseGui.Core.Application.Communication;
public class S7Client
{
    private readonly Plc _plc;
    private readonly Plc _plcW;
    private readonly Timer _timer;
    public List<Tag> Tags { get; private set; }
    public List<MqttTag> MqttTags { get; private set; }

    public bool IsConnected => _plc.IsConnected;
    public S7Client()
    {
        _plc = new Plc(CpuType.S71200, "192.168.0.1", 0, 1);
        _plcW = new Plc(CpuType.S71200, "192.168.0.1", 0, 1);
        _timer = new Timer(1000);
        _timer.Elapsed += TimerElapsed;
        Tags = new()
        {
            //light and DC motor
            new("controlGreen", "PLC.Vali_IFM.controlGreen" ,null, "DB2.DBX16.0", DateTime.Now),
            new("controlRed", "PLC.Vali_IFM.controlRed", null, "DB2.DBX16.2",  DateTime.Now),
            new("controlYellow", "PLC.Vali_IFM.controlYellow", null, "DB2.DBX16.3",  DateTime.Now),
            new("controlDCMotor", "PLC.Vali_IFM.controlDCMotor", null, "DB2.DBX16.1",  DateTime.Now),
            new("ledGreen", "PLC.Vali_IFM.ledGreen", null, "DB2.DBX0.4", DateTime.Now),
            new("ledRed", "PLC.Vali_IFM.ledRed", null, "DB2.DBX0.5",  DateTime.Now),
            new("ledYellow", "PLC.Vali_IFM.ledYellow", null, "DB2.DBX0.6",  DateTime.Now),
            new("DCMotor", "PLC.Vali_IFM.DCmotor", null, "DB2.DBX0.7",  DateTime.Now),

            //sensor
            new("angleRB3100", "PLC.Vali_IFM.angleRB3100", null, "DB2.DBD2",  DateTime.Now),
            new("countRB3100", "PLC.Vali_IFM.countRB3100", null, "DB2.DBW6",  DateTime.Now),
            new("tempTW2000", "PLC.Vali_IFM.tempTW2000", null, "DB2.DBD8",  DateTime.Now),
            new("statusIF6123", "PLC.Vali_IFM.statusIF6123", null, "DB2.DBX14.1", DateTime.Now),
            new("statusKT5112", "PLC.Vali_IFM.statusKT5112", null, "DB2.DBX14.2", DateTime.Now),
            new("statusO5C500", "PLC.Vali_IFM.statusO5C500", null, "DB2.DBX14.3", DateTime.Now),
            new("statusUGT524", "PLC.Vali_IFM.statusUGT524", null, "DB2.DBX14.0", DateTime.Now),
            new("distanceUGT524", "PLC.Vali_IFM.distanceUGT524", null, "DB2.DBW12", DateTime.Now),
            new("resolutionRB3100", "PLC.Vali_IFM.resolutionRB3100", null, "DB8.DBW20", DateTime.Now),

            //inverter 
            new("startup", "PLC.Inverter.startup", null, "DB4.DBX6.0", DateTime.Now),
            new("stop", "PLC.Inverter.stop", null, "DB4.DBX6.1", DateTime.Now),
            new("forward", "PLC.Inverter.forward", null, "DB4.DBX6.2", DateTime.Now),
            new("reverse", "PLC.Inverter.reverse", null, "DB4.DBX6.3", DateTime.Now),
            new("setpoint", "PLC.Inverter.setpoint", null, "DB4.DBW8", DateTime.Now),
            new("speed", "PLC.Inverter.speed", null, "DB4.DBW2", DateTime.Now),

            //TempVar
            new("statusInverter", "PLC.Inverter.statusInverter", null, "DB4.DBX10.0", DateTime.Now),
            new("directionForward", "PLC.Inverter.directionForward", null, "DB4.DBX10.1", DateTime.Now),
            new("directionReverse", "PLC.Inverter.directionReverse", null, "DB4.DBX10.2", DateTime.Now),


            //Siemens Demo Case
            new("mode_M", "PLC.Vali_Siemens.mode", null, "DB8.DBX0.0", DateTime.Now),
            new("reset_M", "PLC.Vali_Siemens.reset", null, "DB8.DBX0.1", DateTime.Now),
            new("start_M", "PLC.Vali_Siemens.start", null, "DB8.DBX0.2", DateTime.Now),
            new("forward_M", "PLC.Vali_Siemens.forward", null, "DB8.DBX0.3", DateTime.Now),
            new("backward_M", "PLC.Vali_Siemens.backward", null, "DB8.DBX0.4", DateTime.Now),
            new("home_M", "PLC.Vali_Siemens.setHome", null, "DB8.DBX0.5", DateTime.Now),
            new("temp_led6", "PLC.Vali_Siemens.temp6", null, "DB8.DBX0.6", DateTime.Now),
            new("temp_led7", "PLC.Vali_Siemens.temp7", null, "DB8.DBX0.7", DateTime.Now),
            new("setpoint_speed_M", "PLC.Vali_Siemens.setpointSpeed", null, "DB8.DBD4", DateTime.Now),
            new("setpoint_position_M", "PLC.Vali_Siemens.setpointPosition", null, "DB8.DBD8", DateTime.Now),
            //
            new("led0", "PLC.Vali_Siemens.led0", null, "DB8.DBX2.0", DateTime.Now),
            new("led1", "PLC.Vali_Siemens.led1", null, "DB8.DBX2.1", DateTime.Now),
            new("led2", "PLC.Vali_Siemens.led2", null, "DB8.DBX2.2", DateTime.Now),
            new("led3", "PLC.Vali_Siemens.led3", null, "DB8.DBX2.3", DateTime.Now),
            new("led4", "PLC.Vali_Siemens.led4", null, "DB8.DBX2.4", DateTime.Now),
            new("led5", "PLC.Vali_Siemens.led5", null, "DB8.DBX2.5", DateTime.Now),
            new("led6", "PLC.Vali_Siemens.led6", null, "DB8.DBX2.6", DateTime.Now),
            new("led7", "PLC.Vali_Siemens.led7", null, "DB8.DBX2.7", DateTime.Now),
            new("current_speed_M", "PLC.Vali_Siemens.currentSpeed", null, "DB8.DBD12", DateTime.Now),
            new("current_position_M", "PLC.Vali_Siemens.currentPosition", null, "DB8.DBD16", DateTime.Now)
        };
        
    }
    public async Task Connect()
    {
        await _plc.OpenAsync();
        await _plcW.OpenAsync();
        _timer.Enabled = true;
    }
    public void Disconnect()
    {
        _timer.Enabled = false;
        _plc.Close();
        _plcW.Close();
    }
    public async void TimerElapsed(object? sender, EventArgs args)
    {
        foreach (var tag in Tags)
        {
            var value = await _plc.ReadAsync(tag.address);
            if (tag.name is "angleRB3100" or "tempTW2000" or "current_speed_M" or "current_position_M")
            {
                tag.value = S7.Net.Conversion.ConvertToFloat(Convert.ToUInt32(value));
            }
            else
            {
                tag.value = value;
            }
            MqttTags = Tags.Select(e => new MqttTag(
                    e.name,
                    e.value,
                    e.timestamp)).ToList();
        }
    }
    public async void WritePLC(string address, object value)
    {
        await _plcW.WriteAsync(address, value);
    }

    public async void WriteNumberPLC(string address, object value)
    {
        var number = Convert.ToUInt16(value);
        await _plcW.WriteAsync(address, number);
    }
    
    public object? GetTagValue(string tagName)
    {
        return Tags.First(x => x.name == tagName).value;
    }

    public string GetTagAddress(string tagName)
    {
        return Tags.First(x => x.name == tagName).address;
    }

    public MqttTag GetTag(string tagName)
    {
        return MqttTags.Find(x => x.name == tagName);
        
    }
    //var mqtttag = Tags.Select(x => new MqttTag(x.name, x.value, x.timestamp));
    //    return (MqttTag) mqtttag;


}