#ifndef HEADER_LINENUMBERS_INCLUDED
    #define HEADER_LINENUMBERS_INCLUDED
    #include <stdio.h>

    void withNumbers(char *p, int numLines, int paramlenght)
    {
        int whichLine = numLines;
        for (int i = (numLines*(paramlenght+2))-1; i>=0;i--)
        {
            if(i % (paramlenght+2) == 6)
            {
                printf("%d ", (whichLine));
                whichLine--;
            }
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