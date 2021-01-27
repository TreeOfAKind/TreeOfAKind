import 'dart:io';

import 'package:flutter/material.dart';

class Avatar extends StatelessWidget {
  const Avatar(
      {Key key, this.photo, @required this.avatarSize, this.backgroundColor})
      : super(key: key);

  final String photo;
  final double avatarSize;
  final Color backgroundColor;

  @override
  Widget build(BuildContext context) {
    return CircleAvatar(
      radius: avatarSize,
      backgroundColor: photo == null ? backgroundColor : null,
      backgroundImage: photo != null
          ? Uri.parse(photo).isAbsolute
              ? NetworkImage(photo)
              : Image.file(File(photo)).image
          : null,
      child:
          photo == null ? Icon(Icons.person_outline, size: avatarSize) : null,
    );
  }
}
