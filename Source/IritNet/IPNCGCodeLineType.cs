namespace IritNet
{
    public enum IPNCGCodeLineType
    {
        IP_NC_GCODE_LINE_COMMENT = 0,
        IP_NC_GCODE_LINE_NONE,

        IP_NC_GCODE_LINE_MOTION_G0FAST,     /* G0 line */
        IP_NC_GCODE_LINE_MOTION_G1LINEAR,   /* G1 line */
        IP_NC_GCODE_LINE_MOTION_G2CW,       /* G2 line */
        IP_NC_GCODE_LINE_MOTION_G3CCW,      /* G3 line */
        IP_NC_GCODE_LINE_MOTION_OTHER,      /* Unsupported motion line */

        IP_NC_GCODE_LINE_NON_MOTION
    }
}