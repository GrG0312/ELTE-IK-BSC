#ifndef HEADER_NOLINENUMBERS_INCLUDED
    #define HEADER_NOLINENUMBERS_INCLUDED
    #include <stdio.h>

    void withoutNumbers(char *p, int numLines, int paramlenght)
    {
        for (int i = (numLines*(paramlenght+2))-1; i>=0;i--)
        {
            if(*(p+i) != '\0' && *(p+i) != '\n')
            {
                printf("%c", *(p+i));
            }
            if( i % (paramlenght+2) == 0)
            {
                printf("\n");
            }
        }
    }











#endif