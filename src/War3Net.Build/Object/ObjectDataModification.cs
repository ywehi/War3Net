﻿// ------------------------------------------------------------------------------
// <copyright file="ObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed class ObjectDataModification
    {
        private int _id;
        private object _value;
        private ObjectDataType _type;

        private int? _level; // For doodads, this number indicates the variation.
        private int _pointer;

        private int _sanityCheck;

        public ObjectDataModification(int id, int value)
            : this(id, value, ObjectDataType.Int, null)
        { }

        public ObjectDataModification(int id, string value)
            : this(id, value, ObjectDataType.String, null)
        { }

        public ObjectDataModification(int id, int level, int value)
            : this(id, value, ObjectDataType.Int, level)
        { }

        public ObjectDataModification(int id, int level, string value)
            : this(id, value, ObjectDataType.String, level)
        { }

        private ObjectDataModification(int id, object value, ObjectDataType type, int? level, int pointer = 0)
            : this()
        {
            _id = id;
            _value = value;
            _type = type;

            _level = level;
            _pointer = pointer;
            _sanityCheck = 0;
        }

        private ObjectDataModification()
        {
        }

        public int Id => _id;

        public object Value => _value;

        public ObjectDataType Type => _type;

        public int? Level => _level;

        public static ObjectDataModification Parse(Stream stream, int oldId, int newId, bool readLevelData, bool leaveOpen = false)
        {
            var data = new ObjectDataModification();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._id = reader.ReadInt32();
                data._type = (ObjectDataType)reader.ReadInt32();

                if (readLevelData)
                {
                    data._level = reader.ReadInt32();
                    data._pointer = reader.ReadInt32();
                }

                switch (data._type)
                {
                    case ObjectDataType.Int:
                        data._value = reader.ReadInt32();
                        break;

                    case ObjectDataType.Real:
                    case ObjectDataType.Unreal:
                        data._value = reader.ReadSingle();
                        break;

                    case ObjectDataType.Bool:
                        data._value = reader.ReadBoolean();
                        break;

                    case ObjectDataType.Char:
                        data._value = reader.ReadChar();
                        break;

                    case ObjectDataType.String:
                        data._value = reader.ReadChars();
                        break;
                }

                var sanityCheck = reader.ReadInt32();
                if (sanityCheck != 0 && sanityCheck != oldId && sanityCheck != newId)
                {
                    throw new InvalidDataException();
                }

                data._sanityCheck = sanityCheck;
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_id);
            writer.Write((int)_type);

            if (_level.HasValue)
            {
                writer.Write(_level.Value);
                writer.Write(_pointer);
            }

            switch (_type)
            {
                case ObjectDataType.Int:
                    writer.Write((int)_value); break;

                case ObjectDataType.Real:
                case ObjectDataType.Unreal:
                    writer.Write((float)_value); break;

                case ObjectDataType.Bool:
                    writer.Write((bool)_value); break;

                case ObjectDataType.Char:
                    writer.Write((char)_value); break;

                case ObjectDataType.String:
                    writer.WriteString((string)_value); break;
            }

            writer.Write(_sanityCheck);
        }
    }
}