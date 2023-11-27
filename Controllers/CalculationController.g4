grammar CalculationController;

/*
* Parser Rules
*/

compileUnit : expression EOF;

expression :
    LPAREN expression RPAREN #ParenthesizedExpr
    | expression EXPONENT expression #ExponentialExpr
    | expression operatorToken=(MULTIPLY | DIVIDE) expression #MultiplicativeExpr
    | expression operatorToken=(ADD | SUBTRACT) expression #AdditiveExpr
    | expression operatorToken=(EQUALS | LESS_THAN | GREATER_THAN) expression #ComparisonExpr
    | NOT expression #NotExpr
    | MAX LPAREN expression ',' expression RPAREN #MaxExpr
    | MIN LPAREN expression ',' expression RPAREN #MinExpr
    | NUMBER #NumberExpr
    | IDENTIFIER #IdentifierExpr
    | BOOLEAN #BoolExpr
    ;

/*
* Lexer Rules
*/

NUMBER : INT ('.' INT)?;
IDENTIFIER : [a-zA-Z]+[1-9][0-9]*;
BOOLEAN : ('T' | 't')('R' | 'r')('U' | 'u')('E' | 'e') 
        | ('F' | 'f')('A' | 'a')('L' | 'l')('S' | 's')('E' | 'e');

INT : ('0'..'9')+;

ADD : '+';
SUBTRACT : '-';
MULTIPLY : '*';
DIVIDE : '/';
EXPONENT : '^';
LPAREN : '(';
RPAREN : ')';
EQUALS : '=';
LESS_THAN : '<';
GREATER_THAN : '>';
MAX : ('M' | 'm')('A' | 'a')('X' | 'x');
MIN : ('M' | 'm')('I' | 'i')('N' | 'n');
NOT : ('N' | 'n')('O' | 'o')('T' | 't');

WS : [ \t\r\n] -> channel(HIDDEN);
