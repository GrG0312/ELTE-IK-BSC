#ifndef HEADER_KONZOLOS_INCLUDED
    #define HEADER_KONZOLOS_INCLUDED
    #include <stdio.h>
    #include <string.h>
    #include <stdlib.h>
    #include "finememory.h"
    #include "linenumbers.h"
    #include "nolinenumbers.h"

    void konzolosMegoldas(char paramsorsz[], int paramlenght)
    {
        char *p = calloc((paramlenght+2), sizeof(char));
        memCheck(p);
        int numLines = 0;
        char *line = calloc((1000), sizeof(char));
        fgets(line,1000,stdin);

        while ( strcmp(line, "\n") != 0)
        {
            for (int i = 0; i < paramlenght; i++)
            {
                *(p+i + (numLines*(paramlenght+2))) = *(line+i);
            }
            *(p+ (numLines*(paramlenght+2))  +5) = '\n';
            *(p+ (numLines*(paramlenght+2))  +6) = '\0';
            numLines = numLines + 1;
            char * tmp = realloc(p, (numLines+1)*(paramlenght+2)*sizeof(char));
            memCheck(tmp);
            for (int i = 0; i< (paramlenght+2);i++)
            {
                *(tmp+i+(numLines*(paramlenght+2))) = '\0';
            }
            p = tmp;
            fgets(line,1000, stdin);
        }
        if (strcmp(paramsorsz, "linenums")==0)
        {
            withNumbers(p, numLines, paramlenght);
        }
        else if ( strcmp(paramsorsz, "nolinenums") == 0)
        {
            withoutNumbers(p, numLines, paramlenght);
        }
        free(p);
    }
#endif