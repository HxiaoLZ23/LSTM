   M S B A M L   `   `   `  ����   LSTM.UI"    LSTM.UI.Views.RegisterWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$  用户注册��$	��600��$	��400�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�   MaterialDesignPaper#���     MaterialDesignBody#���    MaterialDesignFont#���  .���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�#  MaterialDesignThemes.Wpf.Card  $��0��$��20,20,20,10q�+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�.����  �� Orientation$ Vertical=�$��20q�.+�'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ AccountPlus=�$��48��$��48��$��Center=�  PrimaryHueMidBrush#���  �� $��创建新账户��$��20(�$
��Bold"�$��Center=�$��0,10,0,0q�  $��1��$��20,10,20,10q�$ Depth2=�.���� $��30q�.+��� )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	用户名��   
IsFloating$
 True�� ! MaterialDesignOutlinedTextBox#��[ $��0,0,0,20q����� *
Username+	 �� UpdateSourceTrigger$	 PropertyChanged=��� $ 邮箱地址��$
 True��#��[ $��0,0,0,20q����� *Email+$	 PropertyChanged=�G� -   
 G�Name$
 PasswordBox��$ 密码��$
 True�� % !MaterialDesignOutlinedPasswordBox#��[ $��0,0,0,20q�G� -   $
 ConfirmPasswordBox��$ 确认密码��$
 True��#��[ $��0,0,0,20q��� 
���  ��$��12(�$��0,0,0,10q����� *ErrorMessage+y��� *ErrorMessage+  StringToVisibilityConverter �� 	Converter# [ �� 
��� � �$��12(�$��0,0,0,10q����� *SuccessMessage+y��� *SuccessMessage+# [ �� $��注册��  MaterialDesignRaisedButton#��[  �� 	IsDefault$
 True��$��40��$��0,10,0,0q����� *RegisterCommand+�  � IsIndeterminate$
 True��$��0,10,0,0q�y��� *	IsLoading+   BooleanToVisibilityConverter# [ �� $��2��$ 
Horizontal=�$��Center=�$��20q�.+��� $��取消��  	 MaterialDesignOutlinedButton#��[	 $��80������ *CancelCommand+