   M S B A M L   `   `   `  ����   WpfApp2     CaloriesCounter.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Calories Counter��$	��450��$	��800�� � Icon$! images/icon_application.png�� � 	Resources  �� 
TargetType �� Property �� Value T� Y �� Triggers :� RoutedEvent	 �� TargetProperty
 W� From	 W� To g� Duration��%  '��       �� " ��.���  � z� T� $ 0Q� :� $ 
MouseEnter��.[��� .���� .��W� $	 RenderTransform.Y�$
 0X�$ -5X�$ 0:0:0.2D�:� $ 
MouseLeave��.[��� .���� .��W� $	 RenderTransform.Y�$
 -5X�$ 0X�$ 0:0:0.2D�.��� ����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���  � Color$ #F5F5F5�� � Offset$ 0Q�-   
StartColor��� $ #D3D3D3��$ 1Q�-   EndColor�� � Triggers :� $ Loaded��.[��� .����  g� RepeatBehavior$ Forever�� g� AutoReverse . .����  �� 
TargetName$ 
StartColor��$	 Color�	 �� To$ #FFCDD2X�$ 0:0:3D��� $ EndColor��$	 Color�$ #C8E6C9X�$ 0:0:3D�.+�o� .@�m� -   $��
Вихід��m� -   $��Про програму��� $��Center=� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��Calories Counter��$��36(�$
��Bold"�$��Center=�$��	0,20,0,20q��� $!��images/icon_application.png��$	��206��$	��176��$��Center=�$	��Top=�$��	0,10,0,10q�$��1�� J� Opacity$ 0Q�z���  �� ScaleX$	 0.8Q� �� ScaleY$	 0.8Q� :� $ Loaded��.[��� .���� .��W� $	 Opacity�$
 0X�$ 1X�$ 0:0:1D�W� $	 RenderTransform.ScaleX�$	
 0.8X�$ 1X�$ 0:0:1D�W� $	 RenderTransform.ScaleY�$	
 0.8X�$ 1X�$ 0:0:1D��� $��2�� �� Orientation$ 
Horizontal=�$��Center=�$��0,10,0,0q�.+��� -   $��Реєстрація��$��37��$	��150��$
��10,0q�$ 0Q�z�T� $ 10Q� :� $ Loaded��.[��� .���� .��W� $	 Opacity�$
 0X�$ 1X�$ 0:0:0.8D�W� $	 RenderTransform.Y�$
 10X�$ 0X�$ 0:0:0.8D��� -   $��Вхід��$��37��$	��150��$
��10,0q�$ 0Q�z�T� $ 10Q� :� $ Loaded��.[��� .���� .��W� $	 Opacity�$
 0X�$ 1X�$ 0:0:0.8D� g� 	BeginTime$ 0:0:0.5X�W� $	 RenderTransform.Y�$
 10X�$ 0X�$ 0:0:0.8D�$ 0:0:0.5X�