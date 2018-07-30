\include "global-vkg.ily"
\pointAndClickOff
\header{
title = "Sva ljubavi mi, Isuse"
othersource = ""
composer = "Ljubomir Galetić"
source = "PGPN 186"
tagline = ""
}
keyTime = { \key c \major \time 3/2 }

preludeS = \relative e' {
\clef "treble" \keyTime e2. d4 e f |
g a f g e2 \fermata \bar "||"
}
preludeA = \relative c' {
\clef "treble" \keyTime c2. b4 c d |
e2 d c \bar "||"
}
preludeB = \relative c' {
\clef "bass" \keyTime c4 b a2 g4 f |
e2 f
<<
  { g2 }
  \\
  { c,2 \fermata }
>>
\bar "||"
}

\markup \fill-line {
\hspace #1
\score {
\new GrandStaff <<
\new Staff = "upper" <<
\new Voice << \voiceOne \preludeS >>
\new Voice << \voiceTwo \preludeA >>
>>

\new Staff = "lower" <<
\new Voice << \preludeB >>
>>

>>

\layout {}
}
}

voiceS = \relative e' {
\clef "treble" \keyTime e2 e4 d e f |
g2 g e \breathe |
f2 f4 e f g |
a2 a g \breathe |
a2 a4 g a b |
c2 a g \breathe |
e g4 g g f |
e2 d c \fermata \bar "||"
}
voiceA = \relative c' {
\clef "treble" \keyTime c2 c4 b c d |
e2 e c |
d d4 c d e |
f2 f e |
f f4 f f f |
e2 f e |
c e4 e e d |
c2 b c \bar "||"
}

organS = \relative e' {
\clef "treble" \keyTime e2 ( e4 d e f |
g2 g e) \breathe |
f2 ( f4 e f g |
a2 a g) \breathe |
a2 ( a4 g a b |
c2 a g) \breathe |
e ( g4 g g f |
e2 d c) \fermata \bar "||"
}
organA = \relative c' {
\clef "treble" \keyTime c2 c4 b c d |
e2 e c |
d d4 c d e |
f2 f e |
f f4 f f f |
e2 f e |
c e4 e e d |
c2 b c \bar "||"
}
organT = \relative g {
\clef "bass" \keyTime g1 ~ g4 a |
b1 a2 |
a1 ~ a4 b |
c2 b c |
c1 c4 d |
c1 b2 |
a1 g2 ~ |
g4 a d, e e2 \bar "||"
}
organB = \relative c {
\clef "bass" \keyTime c2 ( g g'4 f |
e2 g a) |
d, ( a a'4 g |
f2 g c) |
f, ( d a'4 g |
a2 f g) |
a ( d, g |
g,1 c2) \fermata \bar "||"
}

\score {
<<
\new Staff = "upper" <<
\new Voice = "voiceS" << \voiceOne \voiceS >>
\new Voice << \voiceTwo \voiceA >>
>>

\new GrandStaff <<
\new Staff = "upper" <<
\new Voice << \voiceOne \organS >>
\new Voice << \voiceTwo \organA >>
>>

\new Staff = "lower" <<
\new Voice << \voiceOne \organT >>
\new Voice << \voiceTwo \organB >>
>>

>>

>>
}

