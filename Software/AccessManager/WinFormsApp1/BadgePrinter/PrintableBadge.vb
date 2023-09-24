Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Public Class PrintableBadge

    Public BigCard = New SizeF(85.5, 54)
    Public SmallCard As New SizeF(30,22)
    Public CoinCard As New SizeF(25, 25) ' Assumendo che CoinCard rappresenti un raggio

    Private UserData As UserData
    ' Costruttore della classe
    Public Sub New(userData As UserData)
        Me.UserData = userData
    End Sub


    ' Funzione per generare un'immagine del badge
    Public Function GenerateBadgeImage(cardSizeInMillimeters As SizeF, Optional ppi As Single = 300) As Image
        ' Calcola la larghezza massima in pixel basandoti sulla densità dei pixel (PPI)
        Dim maxTextWidthInPixels As Single = cardSizeInMillimeters.Width * ppi / 25.4F
        Dim maxTextHeightInPixels As Single = cardSizeInMillimeters.Height * ppi / 25.4F

        ' Crea un'immagine vuota del badge
        Dim badgeImage As New Bitmap(CInt(maxTextWidthInPixels), CInt(maxTextHeightInPixels))
        Dim g As Graphics = Graphics.FromImage(badgeImage)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Definisci i font e i pennelli per il testo
        Dim nameFont As New Font("Arial", 16, FontStyle.Bold) ' Carattere del nome più grande
        Dim otherFont As New Font("Arial", 14) ' Carattere per le altre scritte
        Dim nameBrush As New SolidBrush(Color.DarkBlue) ' Colore del nome
        Dim schoolBrush As New SolidBrush(Color.DarkGreen) ' Colore della scuola di ballo
        Dim idBrush As New SolidBrush(Color.DarkRed) ' Colore dell'ID

        Dim borderColor As Color = Color.Black
        Dim borderPen As New Pen(borderColor, 1)

        Dim imagePath As String = (Path_Progetto & "\conf\pic\" & UserData.PicPath)
        Select Case cardSizeInMillimeters
            Case BigCard
                borderPen = New Pen(borderColor, 4)
                If UserData.Name.Length < 16 Then
                    nameFont = New Font("Arial", 60, FontStyle.Bold) ' Carattere del nome più grande
                ElseIf UserData.Name.Length < 24 Then
                    nameFont = New Font("Arial", 52, FontStyle.Bold) ' Carattere del nome più grande
                ElseIf UserData.Name.Length < 30 Then
                    nameFont = New Font("Arial", 50, FontStyle.Bold) ' Carattere del nome più grande
                End If
                otherFont = New Font("Arial", 26) ' Carattere per le altre scritte
                Dim ScoolFont As New Font("Arial", 28, FontStyle.Bold)
                ' Disegna un bordo grigio intorno al badge
                g.DrawRectangle(borderPen, 0, 0, badgeImage.Width - 1, badgeImage.Height - 1)
                ' Carica e disegna l'immagine del profilo (se è specificato un percorso)
                DrawImageInRectangle(g, imagePath, maxTextWidthInPixels * 0.4F, maxTextHeightInPixels * 0.7F, borderPen)

                ' Disegna il nome accanto all'immagine con il carattere del 15% più grande
                Dim nameRectangle As New RectangleF(maxTextWidthInPixels * 0.45F, 10, maxTextWidthInPixels * 0.57F, maxTextHeightInPixels - 20)
                g.DrawString(UserData.Name, nameFont, nameBrush, nameRectangle)
                ' Aggiungi informazioni aggiuntive
                Dim yOffset As Single = 200 ' Spazio verticale tra le linee di testo

                ' Sesso
                g.DrawString("Sesso: " & UserData.Gender, otherFont, Brushes.Black, maxTextWidthInPixels * 0.45F, yOffset)
                yOffset += 45

                ' Data di iscrizione e compleanno
                g.DrawString("Iscrizione : " & UserData.RegDate.ToShortDateString(), otherFont, Brushes.Black, maxTextWidthInPixels * 0.45F, yOffset)
                yOffset += 45
                g.DrawString("Compleanno: " & UserData.Birthday.ToShortDateString(), otherFont, Brushes.Black, maxTextWidthInPixels * 0.45F, yOffset)
                yOffset += 45

                ' Scuola di ballo
                g.DrawString("Scuola : " & UserData.School, ScoolFont, schoolBrush, maxTextWidthInPixels * 0.45F, yOffset)
                yOffset += 90

                ' Nome del badge sotto la voce ID con colore diverso
                g.DrawString("ID: " & UserData.Badge.ToString(), otherFont, idBrush, maxTextWidthInPixels * 0.45F, yOffset)
                yOffset += 50
                g.DrawString("Nr: " & UserData.Id, otherFont, Brushes.Black, maxTextWidthInPixels * 0.02F, yOffset)

            Case SmallCard
                ' Disegna un bordo grigio intorno al badge
                g.DrawRectangle(borderPen, 0, 0, badgeImage.Width - 1, badgeImage.Height - 1)

                ' Calcola l'offset percentuale
                Dim offsetPercent As Single = 0.02F ' 2%

                ' Calcola l'altezza dell'immagine considerando l'offset dal bordo superiore
                Dim imageHeightWithOffset As Single = maxTextHeightInPixels * (0.7F - offsetPercent)


                ' Carica e disegna l'immagine del profilo (se è specificato un percorso)
                DrawImageInRectangle(g, imagePath, maxTextWidthInPixels * 0.45F, maxTextHeightInPixels * 0.7F, borderPen)

                ' Calcola l'offset orizzontale per il nome
                Dim nameXOffset As Single = maxTextWidthInPixels * 0.45F + maxTextWidthInPixels * offsetPercent

                ' Calcola l'altezza massima per il testo del nome in base all'altezza dell'immagine con offset
                Dim maxNameTextHeight As Single = imageHeightWithOffset

                ' Crea un rettangolo per il nome in base all'altezza massima calcolata
                Dim nameRectangle As New RectangleF(nameXOffset, maxTextHeightInPixels * offsetPercent, maxTextWidthInPixels * 0.4F, maxNameTextHeight)

                ' Disegna il nome accanto all'immagine con l'offset
                g.DrawString(UserData.Name, nameFont, nameBrush, nameRectangle)

                ' Calcola l'offset orizzontale dalla sinistra considerando la dimensione dell'immagine e l'offset percentuale
                Dim xOffset As Single = maxTextWidthInPixels * 0.5F + maxTextWidthInPixels * (1 + offsetPercent)
                'calcola il nuovo XOffset
                xOffset = maxTextWidthInPixels * offsetPercent

                ' Calcola l'offset verticale per le altre informazioni
                Dim yOffset As Single = maxTextHeightInPixels * (0.55F + offsetPercent)

                ' Calcola la distanza dalla parte inferiore dell'immagine per la data di iscrizione
                Dim dateOffset As Single = maxTextHeightInPixels * 0.02F
                ' Calcola la larghezza del testo "Iscritta"
                Dim iscrittaWidth As Single = g.MeasureString("Iscrizione : ", otherFont).Width

                ' Posiziona la data di iscrizione sotto l'immagine con l'offset
                g.DrawString("Iscrizione : " & UserData.RegDate.ToShortDateString(), otherFont, Brushes.Black, xOffset, (imageHeightWithOffset + dateOffset) * (1 + offsetPercent) * 1.05F)

                ' Calcola l'offset orizzontale per il testo "Sesso"
                Dim sessoXOffset As Single = (xOffset + g.MeasureString("Iscrizione : " & UserData.RegDate.ToShortDateString(), otherFont).Width + (maxTextWidthInPixels * offsetPercent)) * 1.2F ' aggiunge un offset del 20%

                ' Aggiungi il sesso accanto alla data di iscrizione
                g.DrawString("Sesso: " & UserData.Gender, otherFont, Brushes.Black, sessoXOffset, (imageHeightWithOffset + dateOffset) * (1.0F + offsetPercent) * 1.05F)

                ' Aggiungi la scuola di ballo accanto al sesso
                yOffset += 25 ' Spazio verticale tra le linee di testo
                g.DrawString("Scuola : " & UserData.School, otherFont, schoolBrush, maxTextWidthInPixels * 0.5F, maxTextWidthInPixels * 0.4F)

                ' Aggiungi l'ID accanto alla scuola di ballo
                yOffset += 55 ' Spazio verticale tra le linee di testo
                g.DrawString("ID : " & UserData.Badge.ToString(), otherFont, idBrush, xOffset, yOffset)
                g.DrawString("Nr : " & UserData.Id, otherFont, Brushes.Black, sessoXOffset, yOffset)

            Case CoinCard
                ' Disegna un cerchio come bordo grigio intorno al badge
                Dim circleDiameter As Single = maxTextWidthInPixels * 2
                Dim circleX As Single = (badgeImage.Width - circleDiameter) / 2
                Dim circleY As Single = (badgeImage.Height - circleDiameter) / 2
                g.DrawEllipse(borderPen, circleX, circleY, circleDiameter, circleDiameter)

                DrawImageInCircle(g, imagePath, maxTextWidthInPixels * 0.4F, maxTextHeightInPixels * 0.5F, borderPen)


                Dim textWidth As Single = g.MeasureString(UserData.School, otherFont).Width

                ' Calcola l'xOffset per spostare il testo del 10% rispetto all'origine dell'asse X
                Dim xOffsetPercent As Single = 0.15F ' 15% di offset
                Dim xOffset As Single = (maxTextWidthInPixels / 2)
                'calcolo correttamente l'offset
                xOffset = xOffset - (xOffset * xOffsetPercent) - textWidth

                ' Calcola l'yOffset per spostare il testo del 75% rispetto all'origine dell'asse Y
                Dim yOffsetPercent As Single = 0.25F ' 25% di offset
                Dim yOffset As Single = (maxTextHeightInPixels / 2)
                'calcolo correttamente l'offset
                yOffset = yOffset - (xOffset * yOffsetPercent)

                ' Ora puoi utilizzare xOffset e yOffset per posizionare il testo all'interno del badge con gli offset desiderati.
                Dim schoolRectangle As New RectangleF(xOffset, yOffset, maxTextWidthInPixels * 0.6F, maxTextHeightInPixels - yOffset)

                ' Imposta il colore di sfondo
                Dim backgroundColor As Color = Color.Aqua ' Il colore di sfondo desiderato

                ' Crea un rettangolo con le stesse dimensioni del testo
                Dim backgroundRectangle As New RectangleF(xOffset * (1 - 0.2), yOffset, textWidth * 1.4, 25)

                ' Disegna il rettangolo di sfondo
                g.FillRectangle(New SolidBrush(backgroundColor), backgroundRectangle)

                ' Disegna la scuola dell'allievo con il colore del testo
                g.DrawString(UserData.School, nameFont, schoolBrush, schoolRectangle)
        End Select

        ' Rilascia le risorse
        g.Dispose()

        Return badgeImage
    End Function
    Public Sub DrawImageInRectangle(g As Graphics, imagePath As String, rectangleWidthInPixels As Single, rectangleHeightInPixels As Single, borderPen As Pen, Optional imageSizeMultiplier As Single = 1.0F, Optional offsetPercent As Single = 0.02F)
        If Not String.IsNullOrEmpty(imagePath) AndAlso File.Exists(imagePath) Then
            Dim profileImage As New Bitmap(imagePath)

            ' Calcola la dimensione dell'immagine basata sul moltiplicatore specificato
            Dim targetWidth As Single = rectangleWidthInPixels * imageSizeMultiplier
            Dim targetHeight As Single = rectangleHeightInPixels * imageSizeMultiplier

            ' Calcola le coordinate X e Y con offset
            Dim offsetX As Single = rectangleWidthInPixels * offsetPercent
            Dim offsetY As Single = rectangleHeightInPixels * offsetPercent
            Dim imageX As Single = offsetX
            Dim imageY As Single = offsetY

            ' Calcola il rettangolo dell'immagine
            Dim imageRectangle As New RectangleF(imageX, imageY, targetWidth, targetHeight)

            ' Disegna il rettangolo
            g.DrawRectangle(borderPen, imageRectangle.X, imageRectangle.Y, imageRectangle.Width, imageRectangle.Height)

            ' Disegna l'immagine all'interno del rettangolo
            g.DrawImage(profileImage, imageRectangle)
            profileImage.Dispose()
        End If
    End Sub






    Private Sub DrawImageInCircle(g As Graphics, imagePath As String, maxTextWidthInPixels As Single, maxTextHeightInPixels As Single, borderPen As Pen)
        If Not String.IsNullOrEmpty(imagePath) AndAlso File.Exists(imagePath) Then
            Dim profileImage As New Bitmap(imagePath)

            ' Crea un cerchio vuoto
            Dim circlePath As New GraphicsPath()
            circlePath.AddEllipse(0, 0, maxTextWidthInPixels * 2, maxTextWidthInPixels * 2)

            ' Calcola la dimensione dell'immagine per adattarla al cerchio
            Dim imageSize As SizeF
            If profileImage.Width > profileImage.Height Then
                Dim ratio As Single = (maxTextWidthInPixels * 2) / profileImage.Width
                imageSize = New SizeF(maxTextWidthInPixels * 2, profileImage.Height * ratio)
            Else
                Dim ratio As Single = (maxTextWidthInPixels * 2) / profileImage.Height
                imageSize = New SizeF(profileImage.Width * ratio, maxTextWidthInPixels * 2)
            End If

            ' Posiziona l'immagine al centro del cerchio
            Dim imageX As Single = (maxTextWidthInPixels * 2 - imageSize.Width) / 2
            Dim imageY As Single = (maxTextWidthInPixels * 2 - imageSize.Height) / 2

            ' Disegna il cerchio
            g.DrawEllipse(borderPen, 0, 0, maxTextWidthInPixels * 2, maxTextWidthInPixels * 2)

            ' Ritaglia l'immagine per adattarla al cerchio
            g.SetClip(circlePath)
            g.DrawImage(profileImage, imageX, imageY, imageSize.Width, imageSize.Height)

            profileImage.Dispose()
        End If
    End Sub



End Class
