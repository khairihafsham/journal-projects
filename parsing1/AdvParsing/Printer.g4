grammar Printer;

stat : print+
     ;

print : 'print' '(' expr ')'
      ;

expr : expr '+' expr # Add
     | INT           # Int
     ;

INT : [0-9]+
    ;

WS : [ \t\n\r]+ -> skip
   ;
