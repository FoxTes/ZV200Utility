using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZV200Utility.Core.Enums;
using ZV200Utility.Services.DeviceManager.Model;

namespace ZV200Utility.Services.DeviceManager
{
    /// <summary>
    /// Менеджер управления подключения к прибору.
    /// </summary>
    public interface IDeviceManager
    {
        /// <summary>
        /// Событие, возникающие при опросе регистров.
        /// </summary>
        event EventHandler<IReadOnlyList<SensorInfoArgs>> RegistersRequested;

        /// <summary>
        /// Событие, возникающие при изменение состояния подключения к прибору.
        /// </summary>
        event EventHandler StatusConnectChanged;

        /// <summary>
        /// Получает значение, указывающее состояние соединения с прибором.
        /// </summary>
        StatusConnect StatusConnect { get; }

        /// <summary>
        /// Возвращает или задает настройки порта.
        /// </summary>
        SettingModbusArgs SettingModbus { get; set; }

        /// <summary>
        /// Возвращает настройки прибора.
        /// </summary>
        SettingDeviceArgs SettingDevice { get;  }

        /// <summary>
        /// Задает общие настройки прибора.
        /// </summary>
        /// <param name="settingDevice">1.</param>
        /// <returns></returns>
        Task SetSettingDevice(SettingDeviceArgs settingDevice);
    }
}