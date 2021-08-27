﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using FastEnumUtility;
using Notifications.Wpf.Core;
using Prism.Regions;
using ZV200Utility.Core.Enums;
using ZV200Utility.Core.Helpers;
using ZV200Utility.Core.Mvvm;
using ZV200Utility.Core.Services;
using ZV200Utility.Services.DeviceManager;
using ZV200Utility.Services.DeviceManager.Model;
using ZV200Utility.Services.Notification;
using ZV200Utility.Services.SerialPortScanner;
using ZV200Utility.Services.SerialPortScanner.Models;

namespace ZV200Utility.Modules.Setting.ViewModels
{
    /// <summary>
    /// Модель представления для SettingWindow.xaml.
    /// </summary>
    public class SettingWindowViewModel : RegionViewModelBase
    {
        private readonly INavigationJournal _navigationJournal;
        private readonly IDeviceManager _deviceManager;
        private readonly INotification _notification;

        private bool _isSettingChanged;
        private bool _isSettingDeviceChanged;

        private int _addressDeviceSelected;
        private string _serialPortSelected;
        private BaudRate _baudRateSelected;

        private IEnumerable<string> _serialPortSource = SerialPort.GetPortNames();
        private RelayOperatingMode _relayFunctionSelected;
        private bool _relayLogicStatus;
        private bool _roundFunctionStatus;
        private bool _inputDiscreteLogicStatus;
        private bool _statusConnect;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SettingWindowViewModel"/>.
        /// </summary>
        /// <param name="serialPortScanner">Сканер портов.</param>
        /// <param name="navigationJournal">Журнал.</param>
        /// <param name="deviceManager">Менеджер прибора.</param>
        /// <param name="notification">Уведомления.</param>
        public SettingWindowViewModel(
            ISerialPortScanner serialPortScanner,
            INavigationJournal navigationJournal,
            IDeviceManager deviceManager,
            INotification notification)
        {
            _navigationJournal = navigationJournal;
            _notification = notification;
            _deviceManager = deviceManager;
            _deviceManager.StatusConnectChanged += OnDeviceManagerOnStatusConnectChanged;

            serialPortScanner.SerialPortChanged += SerialPortScannerOnSerialPortChanged;
        }

        /// <summary>
        /// Адреса устройств в сети MODBUS.
        /// </summary>
        public IReadOnlyList<int> AddressDeviceSource { get; } = Enumerable
            .Range(1, 247)
            .ToList();

        /// <summary>
        /// Выбранный адрес устройства.
        /// </summary>
        public int AddressDeviceSelected
        {
            get => _addressDeviceSelected;
            set => _isSettingChanged = SetProperty(ref _addressDeviceSelected, value);
        }

        /// <summary>
        /// Адреса устройств в сети MODBUS.
        /// </summary>
        public IEnumerable<string> SerialPortSource
        {
            get => _serialPortSource;
            private set => _isSettingChanged = SetProperty(ref _serialPortSource, value);
        }

        /// <summary>
        /// Выбранный адрес устройства.
        /// </summary>
        public string SerialPortSelected
        {
            get => _serialPortSelected;
            set => _isSettingChanged = SetProperty(ref _serialPortSelected, value);
        }

        /// <summary>
        /// Выбранный адрес устройства.
        /// </summary>
        public BaudRate BaudRateSelected
        {
            get => _baudRateSelected;
            set => _isSettingChanged = SetProperty(ref _baudRateSelected, value);
        }

        /// <summary>
        /// Функция реле.
        /// </summary>
        public RelayOperatingMode RelayFunctionSelected
        {
            get => _relayFunctionSelected;
            set
            {
                if (SetProperty(ref _relayFunctionSelected, value) && _isSettingDeviceChanged)
                    UpdateDeviceSetting();
            }
        }

        /// <summary>
        /// Логика работы реле.
        /// </summary>
        public bool RelayLogicStatus
        {
            get => _relayLogicStatus;
            set
            {
                if (SetProperty(ref _relayLogicStatus, value) && _isSettingDeviceChanged)
                    UpdateDeviceSetting();
            }
        }

        /// <summary>
        /// Функция звуковой сигнализации открытия двери.
        /// </summary>
        public bool SoundFunctionStatus
        {
            get => _roundFunctionStatus;
            set
            {
                if (SetProperty(ref _roundFunctionStatus, value) && _isSettingDeviceChanged)
                    UpdateDeviceSetting();
            }
        }

        /// <summary>
        /// Логика работы дискретного входа.
        /// </summary>
        public bool InputDiscreteLogicStatus
        {
            get => _inputDiscreteLogicStatus;
            set
            {
                if (SetProperty(ref _inputDiscreteLogicStatus, value) && _isSettingDeviceChanged)
                    UpdateDeviceSetting();
            }
        }

        /// <summary>
        /// Статус соединения с прибором.
        /// </summary>
        public bool StatusConnectDevice
        {
            get => _statusConnect;
            set => SetProperty(ref _statusConnect, value);
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationJournal.RegionNavigationJournal = navigationContext.NavigationService.Journal;
            _navigationJournal.UpdateNavigationJournal();

            _isSettingDeviceChanged = false;
            SetDefaultValue();
            _isSettingChanged = false;
            _isSettingDeviceChanged = true;
        }

        /// <inheritdoc />
        public override async void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (!_isSettingChanged)
                return;

            UpdateAppSettings();
            UpdateSettings();
            await _notification.ShowAsync("Настройки", "Настройки были сохранены.", NotificationType.Information);
        }

        private void OnDeviceManagerOnStatusConnectChanged(object sender, EventArgs args)
            => StatusConnectDevice = _deviceManager.StatusConnect == StatusConnect.Connected;

        private void SerialPortScannerOnSerialPortChanged(object sender, SerialPortArgs e)
        {
            SerialPortSource = e.SerialPorts;
            SerialPortSelected = SerialPortSource.FirstOrDefault();
        }

        private void SetDefaultValue()
        {
            AddressDeviceSelected = _deviceManager.SettingModbus.AddressDevice;
            SerialPortSelected = _deviceManager.SettingModbus.SerialPort;
            BaudRateSelected = _deviceManager.SettingModbus.BaudRate;

            RelayFunctionSelected = _deviceManager.SettingDevice.RelayFunction;
            RelayLogicStatus = _deviceManager.SettingDevice.RelayLogic;
            SoundFunctionStatus = _deviceManager.SettingDevice.SoundFunction;
            InputDiscreteLogicStatus = _deviceManager.SettingDevice.InputDiscreteLogic;
        }

        private void UpdateAppSettings()
        {
            Settings.AddOrUpdateAppSetting("AddressModbusSelected", AddressDeviceSelected);
            Settings.AddOrUpdateAppSetting("ComPortSelected", SerialPortSelected);
            Settings.AddOrUpdateAppSetting("BaudRateSelected", BaudRateSelected.GetEnumMemberValue());
        }

        private void UpdateSettings()
        {
            _deviceManager.SettingModbus = new SettingModbusArgs(
                (byte)AddressDeviceSelected,
                SerialPortSelected,
                BaudRateSelected);
        }

        private async void UpdateDeviceSetting()
        {
            try
            {
                await _deviceManager.SetSettingDevice(new SettingDeviceArgs(
                    RelayFunctionSelected,
                    RelayLogicStatus,
                    SoundFunctionStatus,
                    InputDiscreteLogicStatus));
                await _notification.ShowAsync("Настройки", "Настройки прибора были изменены.", NotificationType.Information);
            }
            catch (Exception)
            {
                await _notification.ShowAsync(
                    "Настройки",
                    "Настройки прибора не удалось изменить.",
                    NotificationType.Warning);
            }
        }
    }
}
