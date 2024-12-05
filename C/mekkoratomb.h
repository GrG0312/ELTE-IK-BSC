#ifndef HEADER_MEKKORA_INCLUDED
    #define HEADER_MEKKORA_INCLUDED
    #include <stdio.h>

    int tombmeretMeghataroz( char line[])
    {
        int tm = 0;
        for (int i = 0; line[i]!='\0'; ++i)
        {
            if (line[i] == ' ')
            {
                tm = tm + 1;
            }
        }
        return tm;
    }


#endif