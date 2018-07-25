#(set-global-staff-size 18)

\paper {
    top-margin = 1.0\cm
    bottom-margin = 2.0\cm
    left-margin = 1.0\cm
    right-margin = 1.0\cm
    bookTitleMarkup = 
      \markup {
        \column {
          \fill-line {
            \column {
              \concat {
                \hspace #5
                \fontsize #3 \bold \fromproperty #'header:title
              }
            }
            \hspace #1
          }
          \null
          \fill-line {
            \hspace #1
            \column {
              \italic \fromproperty #'header:othersource
              \italic \fromproperty #'header:poet
              \italic \fromproperty #'header:composer
              \italic \fromproperty #'header:arranger
              \italic \fromproperty #'header:other
              \italic \fromproperty #'header:source
            }
          }
        }
     }
}