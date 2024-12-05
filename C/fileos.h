#ifndef HEADER_FILEOS_INCLUDED
    #define HEADER_FILEOS_INCLUDED
    #include <stdio.h>
    #include <string.h>
    #include <stdlib.h>
    #include "finememory.h"
    #include "linenumbers.h"
    #include "nolinenumbers.h"

    void fileosMegoldas(char paramsorsz[], int paramlenght, FILE *fptr)
    {
        char *p = calloc((paramlenght+2), sizeof(char));
        memCheck(p);
        int numLines = 0;
        char *line = calloc((1000), sizeof(char));
        fgets(line,1000,fptr);
        char first = *line;

        while ( first != EOF)
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
            fgets(line,1000, fptr);
            first = *line;
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