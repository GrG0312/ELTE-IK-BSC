#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "fuggvenyek.h"

void determineIfRight(int length, char ** argv)
    {
        if (length < 3)
        {
            printf("Usage:\n\trev [SHOW LINE NUMBERS] [MAX LINE LENGTH] files...");
            exit(1);
        } else
        {
            if( strcmp(argv[1], "linenums") != 0 && strcmp(argv[1], "nolinenums") != 0 )
            {
                printf("Usage:\n\trev [SHOW LINE NUMBERS] [MAX LINE LENGTH] files...");
                exit(2);
            }
        }
    }

void memCheck( char* ptr )
    {
        if (ptr == NULL)
        {
            printf("\nMemory allocation failed!\n");
            exit(99);
        }
    }

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

void withNumbers(char *p, int numLines, int paramlenght)
    {
        int whichLine = numLines;
        for (int i = (numLines*(paramlenght+2))-1; i>=0;i--)
        {
            if(i % (paramlenght+2) == (paramlenght+1))
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

void fileosMegoldas(char paramsorsz[], int paramlenght, char ** argv, int argc)
    {

        //argv[0] = a.out
        //argv[1] = linenums/nolinenums = paramsorsz
        //argv[2] = 9 = paramlength
        //argv[...] = "fájlnév"

        for (int i = 3; i < argc; i++)
        {
            FILE *fptr = fopen(argv[i], "r");
            if( fptr == NULL)
            {
                fprintf(stderr, "File opening unsuccessful: %s", argv[i]);
            } else
            {
                char *p = calloc((paramlenght+2), sizeof(char));
                memCheck(p);
                int numLines = 0;
                char *line = calloc((2000), sizeof(char));

                while(fgets(line,2000,fptr))
                {
                    for (int j = 0; j < paramlenght && *(line + j) != '\n'; j++)
                    {
                        *(p+ j + (numLines*(paramlenght+2))) = *(line+j);
                    }
                    *(p+ (numLines*(paramlenght+2))  + paramlenght) = '\n';
                    *(p+ (numLines*(paramlenght+2))  + (paramlenght+1)) = '\0';
                    numLines = numLines + 1;

                    char * tmp = realloc(p, (numLines+1)*(paramlenght+2)*sizeof(char));
                    memCheck(tmp);
                    for (int j = 0; j< (paramlenght+2);j++)
                    {
                        *(tmp+j+(numLines*(paramlenght+2))) = '\0';
                    }
                    p = tmp;
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
                free(line);
            }
        }
    }

void konzolosMegoldas(char paramsorsz[], int paramlenght)
    {
        char *p = calloc((paramlenght+2), sizeof(char));
        memCheck(p);
        int numLines = 0;
        char *line = calloc((2000), sizeof(char));
        fgets(line,2000,stdin);

        while ( strcmp(line, "\n") != 0)
        {
            for (int i = 0; i < paramlenght && *(line+i) != '\n'; i++)
            {
                *(p+i + (numLines*(paramlenght+2))) = *(line+i);
            }
            *(p+ (numLines*(paramlenght+2))  + paramlenght) = '\n';
            *(p+ (numLines*(paramlenght+2))  + (paramlenght+1)) = '\0';
            numLines = numLines + 1;
            char * tmp = realloc(p, (numLines+1)*(paramlenght+2)*sizeof(char));
            memCheck(tmp);
            for (int i = 0; i< (paramlenght+2);i++)
            {
                *(tmp+i+(numLines*(paramlenght+2))) = '\0';
            }
            p = tmp;
            fgets(line,2000, stdin);
        }

        if (strcmp(paramsorsz, "linenums")==0)
        {
            withNumbers(p, numLines, paramlenght);
        }
        else if ( strcmp(paramsorsz, "nolinenums") == 0)
        {
            withoutNumbers(p, numLines, paramlenght);
        }
        free(line);
        free(p);
    }