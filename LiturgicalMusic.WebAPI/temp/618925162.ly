\include "global-vkg.ily"
\pointAndClickOff
\header{
title = "Bezgrješnom Srcu"
othersource = "Tekst: Josip Ivić, st."
composer = "Pero Ivanišić-Crnkovački"
source = ""
tagline = ""
}
keyTime = { \key es \major \time 3/4 }

voiceS = \relative g' {
\clef "treble" \keyTime \repeat volta 2 {
  g4^\markup \italic { Umjereno } g as |
  bes2 g4 |
  as g es |
  f2 f4 |
  f f g |
  as2 bes4 |
  c bes f |
  g2 g4 |
}
bes^\markup \italic { Pripjev } g as |
bes2 bes4 |
c as g |
f2 f4 |
g^\markup \italic { Polako } f es |
g2. |
bes4^\markup \italic { Veselije } g as |
bes2 bes4 |
c as g |
f2 f4 |
g^\markup \italic { Polako } g f |
es2. \bar "|."
}
voiceA = \relative e' {
\clef "treble" \keyTime \repeat volta 2 {
  es4 es f |
  g2 es4 |
  f es c |
  bes2 bes4 |
  d d es |
  f2 es4 |
  es d d |
  es2 es4 |
}
g es f |
g2 g4 |
g4 f es |
d2 d4 |
es es es |
es2. |
g4 es f |
g2 g4 |
g4 f es |
d2 d4 |
es es d |
es2. \bar "|."
}

organS = \relative g' {
\clef "treble" \keyTime \repeat volta 2 {
  g2 as4 |
  bes2 g4 |
  as g es |
  f2 f4 |
  f2 g4 |
  as2 bes4 |
  c bes f |
  g2. |
}
bes4 g as |
bes2. |
c4 as g |
f2. |
g4 f es |
g2. |
bes4 g as |
bes2. |
c4 as g |
f2. |
g2 f4 |
es2. \bar "|."
}
organA = \relative e' {
\clef "treble" \keyTime \repeat volta 2 {
  es2 f4 |
  g2 es4 |
  f es c |
  bes4 bes8 [ c] d4 |
  d2 es4 |
  f2 es4 |
  es d d |
  es2. |
}
g4 es f |
g2. |
g4 f es |
d2. |
es2. |
es2. |
g4 es f |
g2. |
g4 f es |
d2. |
es2 d4 |
bes2. \bar "|."
}
organT = \relative b {
\clef "bass" \keyTime \repeat volta 2 {
  bes4 c ces |
  bes2. ~ |
  bes2 a4 |
  f8 [ g] bes2 |
  bes2. |
  c4 bes8 [ as] g4 |
  as f bes |
  bes2. |
}
es2. |
es2. |
c2 a4 |
bes2. |
bes4 as g |
bes2. |
es2. |
es2. |
c2 a4 |
bes2. |
bes4 a as |
g2. \bar "|."
}
organB = \relative e {
\clef "bass" \keyTime \repeat volta 2 {
  es2. |
  es2. |
  d4 es f |
  bes,2 bes4 |
  bes' as g |
  f8 [ es d c] bes4 |
  as4 bes2 |
  es2. |
}
es2. |
es2. |
e4 f2 |
bes2. |
es,2. |
es2. |
es2. |
es2. |
e4 f2 |
bes2. |
es,4 f bes, |
es2. \bar "|."
}

lyricsA = \lyricmode {
\set stanza = "1."
Bez -- grje -- šnom Sr -- cu
ne -- be -- ske Maj -- ke
naj -- lje -- pša pje -- sma
sad nek se o -- ri;
O Sr -- ce pu -- no
pla -- me -- na sve -- tog, u -- že -- zi nas.
O Sr -- ce moć -- no, o Sr -- ce do -- bro,
bu -- di nam spas.
}
lyricsB = \lyricmode {
Sr -- cu, što pu -- no
lju -- ba -- vi žar -- ke,
za nas vijek mo -- li,
za nas vijek go -- ri.
}
lyricsC = \lyricmode {
\set stanza = "2."
O -- vim nas Sr -- cem
Maj -- ka sve lju -- bi,
"k Bo" -- gu nas vo -- di,
mi -- lost nam pro -- si.
}
lyricsD = \lyricmode {
Tko "k nje" -- mu hr -- li,
taj se ne gu -- bi
jer o -- vo Sr -- ce
mir i spas no -- si.
}
lyricsE = \lyricmode {
\set stanza = "3."
U tom je Sr -- cu
sva na -- ša na -- da,
u -- tje -- ha bo -- li,
lijek sva -- koj ra -- ni
}
lyricsF = \lyricmode {
jer o -- vo Sr -- ce
sva -- kog koj stra -- da
pre -- mi -- lo tje -- ši
i od zla bra -- ni.
}
lyricsG = \lyricmode {
\set stanza = "4."
O di -- vno Sr -- ce
ne -- be -- ske Maj -- ke,
na mol -- bu dje -- ce
mi -- lost u -- dije -- li;
}
lyricsH = \lyricmode {
nek zra -- ke tvo -- je
lju -- ba -- vi žar -- ke
za -- pa -- le o -- gnjem
svijet o -- vaj cije -- li.
}

\score {
<<
\new Staff = "upper" <<
\new Voice = "voiceS" << \voiceOne \voiceS >>
\new Voice << \voiceTwo \voiceA >>
>>

\new Lyrics \lyricsto "voiceS" \lyricsA
\new Lyrics \lyricsto "voiceS" \lyricsB
\new Lyrics \lyricsto "voiceS" \lyricsC
\new Lyrics \lyricsto "voiceS" \lyricsD
\new Lyrics \lyricsto "voiceS" \lyricsE
\new Lyrics \lyricsto "voiceS" \lyricsF
\new Lyrics \lyricsto "voiceS" \lyricsG
\new Lyrics \lyricsto "voiceS" \lyricsH

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

