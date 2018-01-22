package ClassLib062;

public class Class001 {
    public static String property() {
        return "ClassLib062" + " " + ClassLib051.Class001.property() + " " + ClassLib027.Class001.property() + " " + ClassLib028.Class001.property();
    }
}
