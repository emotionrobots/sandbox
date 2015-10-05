Public Enum TransmitOptionFlagsEnum
    PanPower = 1
    TiltPower = 2
    Lights = 4
    Output4 = 8
    'ZoomIn = 16 Or 32
    'ZoomOut = 32
    RightDirFwd = 64
    LeftDirFwd = 128
    EncoderReset = 256 * 1
    SpeedControl = 256 * 2
    OverrideFront = 256 * 4
    OverrideRear = 256 * 8
    OverrideBumper = 256 * 16
    ZoomIn = 256 * 64
    ZoomOut = 256 * 128
End Enum
